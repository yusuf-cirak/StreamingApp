using System.Reflection;
using Application.Abstractions;
using Application.Common.Behaviors;
using Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceRegistration
{
    private static void AddRuleServices(this IServiceCollection services, Type type, Assembly assembly,
        Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
    {
        var typeOfRules = type;
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeOfRules) && typeOfRules != t);

        if (addWithLifeCycle is null)
        {
            foreach (Type businessRuleType in types)
            {
                services.AddScoped(businessRuleType);
            }
        }
        else
        {
            foreach (Type businessRuleType in types)
            {
                addWithLifeCycle(services, businessRuleType);
            }
        }
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();

        AddRuleServices(services, typeof(BaseBusinessRules), executingAssembly);

        services.AddScoped<AuthManager>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(executingAssembly));

        // FluentValidation dependency injection
        services.AddValidatorsFromAssembly(executingAssembly);

        // AuthorizationBehavior dependency injection
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
    }
}