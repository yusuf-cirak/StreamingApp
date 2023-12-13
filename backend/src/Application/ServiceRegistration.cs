using System.Reflection;
using Application.Abstractions;
using Application.Common.Behaviors;
using Application.Services.Streamers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceRegistration
{
    private static void AddBusinessRuleServices(this IServiceCollection services, Assembly assembly,
        Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
    {
        var typeOfBusinessRules = typeof(BaseBusinessRules);
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeOfBusinessRules) && typeOfBusinessRules != t);

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

        AddBusinessRuleServices(services, executingAssembly);

        services.AddSingleton<IStreamerService, StreamerService>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(executingAssembly));

        // FluentValidation dependency injection
        services.AddValidatorsFromAssembly(executingAssembly);

        // AuthorizationBehavior dependency injection
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
    }
}