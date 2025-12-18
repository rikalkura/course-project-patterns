using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Application.Common.Behaviors;
using OnlineStore.Application.Common.Mappings;

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

        return services;
    }
}

