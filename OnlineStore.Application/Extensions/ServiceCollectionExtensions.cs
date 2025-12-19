using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Application.Common.Behaviors;
using OnlineStore.Application.Common.Mappings;
using OnlineStore.Application.Patterns.Factory;
using OnlineStore.Application.Patterns.Observer;
using OnlineStore.Application.Patterns.Singleton;
using OnlineStore.Application.Patterns.Strategy;

namespace OnlineStore.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Add MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(ApplicationAssembly.Assembly);
        });

        // Add AutoMapper - scans assembly for profiles
        services.AddAutoMapper((serviceProvider, cfg) =>
        {
            cfg.AddProfile<MappingProfile>();
        }, ApplicationAssembly.Assembly);

        // Add FluentValidation
        services.AddValidatorsFromAssembly(ApplicationAssembly.Assembly);

        // Add validation behavior pipeline
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Register GoF Design Patterns
        RegisterDesignPatterns(services);

        return services;
    }

    private static void RegisterDesignPatterns(IServiceCollection services)
    {
        // Strategy Pattern - Pricing
        services.AddScoped<IPricingStrategy>(sp => 
            new PercentageDiscountStrategy(10)); // Default 10% discount strategy

        // Strategy Pattern - Delivery (will be resolved based on delivery method)
        services.AddScoped<IDeliveryCostStrategy>(sp => 
            new StandardDeliveryStrategy());

        // Factory Pattern - Notifications
        services.AddScoped<INotificationFactory, NotificationFactory>();

        // Factory Pattern - Product Types
        services.AddScoped<IProductTypeFactory, ProductTypeFactory>();

        // Observer Pattern - Order Status
        services.AddScoped<OrderStatusSubject>(sp =>
        {
            var subject = new OrderStatusSubject();
            var notificationFactory = sp.GetRequiredService<INotificationFactory>();
            var logger = LoggerService.Instance;

            // Attach observers
            subject.Attach(new EmailNotificationObserver(notificationFactory));
            subject.Attach(new SmsNotificationObserver(notificationFactory));
            subject.Attach(new LoggingObserver(logger));

            return subject;
        });

        // Singleton Pattern - Logger (registered as singleton via DI)
        services.AddSingleton<ILoggerService>(sp => LoggerService.Instance);
    }
}

