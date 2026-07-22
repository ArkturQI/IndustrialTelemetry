using FluentValidation;
using IndustrialTelemetry.Application.Interfaces;
using IndustrialTelemetry.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IndustrialTelemetry.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITelemetryService, TelemetryService>();
        services.AddScoped<IEquipmentService, EquipmentService>();
        services.AddValidatorsFromAssemblyContaining(typeof(TelemetryService));

        return services;
    }
}