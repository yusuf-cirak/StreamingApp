using System.Reflection;
using Application.Abstractions;
using Application.Common.Behaviors;
using Application.Common.Permissions;
using Application.Common.Services;
using Application.Features.Streams.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceRegistration
{
    private static void RegisterTypedServices(this IServiceCollection services, Type type, Assembly assembly,
        Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
    {
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t);

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

    private static void RegisterInterfaceServices(this IServiceCollection services, Assembly assembly, Type type,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var types = assembly.GetTypes().Where(t =>
            t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == type &&
                !t.IsInterface));

        foreach (Type implementationType in types)
        {
            var interfaceType = implementationType
                .GetInterfaces()
                .FirstOrDefault(@interface =>
                    @interface != type
                    && @interface.GetInterfaces()
                        .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == type));

            if (interfaceType != null)
            {
                services.Add(new ServiceDescriptor(interfaceType, implementationType, serviceLifetime));
            }
        }
    }

    public static void AddApplicationServices(this IServiceCollection services)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();

        RegisterTypedServices(services, typeof(BaseBusinessRules), executingAssembly);

        RegisterInterfaceServices(services, executingAssembly, typeof(IDomainService<>));


        services.AddSingleton<IPermissionService, PermissionService>();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IStreamCacheService, StreamCacheService>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(executingAssembly));

        // FluentValidation dependency injection
        services.AddValidatorsFromAssembly(executingAssembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PermissionBehavior<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LockBehavior<,>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ApiAuthorizationBehavior<,>));
        
    }
}