using IndustrialTelemetry.Domain.Enums;
using IndustrialTelemetry.Domain.Models;
using IndustrialTelemetry.Infrastructure;
using IndustrialTelemetry.Infrastructure.DATA;
using Microsoft.EntityFrameworkCore;

namespace IndustrialTelemetry.API.BackgroundServices;

public class TelemetryAlertWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TelemetryAlertWorker> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(30);

    public TelemetryAlertWorker(IServiceProvider serviceProvider, ILogger<TelemetryAlertWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TelemetryAlertWorker is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckCriticalValuesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking critical values");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }

        _logger.LogInformation("TelemetryAlertWorker is stopping.");
    }

    private async Task CheckCriticalValuesAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TelemetryDbContext>();

        var thresholdTime = DateTime.UtcNow.AddMinutes(-5);
        var recentTelemetry = await dbContext.TelemetryRecords
            .Where(t => t.Timestamp >= thresholdTime)
            .ToListAsync(cancellationToken);

        var groupedByEquipment = recentTelemetry.GroupBy(t => t.EquipmentId);

        foreach (var group in groupedByEquipment)
        {
            var equipmentId = group.Key;
            var latestRecord = group.OrderByDescending(t => t.Timestamp).FirstOrDefault();

            if (latestRecord == null) continue;

            if (latestRecord.MetricType == "Temperature" && latestRecord.Value > 100)
            {
                var equipment = await dbContext.Equipments
                    .FirstOrDefaultAsync(e => e.Id == equipmentId, cancellationToken);

                if (equipment != null && equipment.Status != EquipmentStatus.Critical)
                {
                    equipment.UpdateStatus(EquipmentStatus.Critical);

                    var alert = new Alert(equipmentId, $"Critical temperature detected: {latestRecord.Value}°C");

                    dbContext.Alerts.Add(alert);
                    await dbContext.SaveChangesAsync(cancellationToken);

                    _logger.LogWarning("Critical alert created for equipment {EquipmentId}: {Message}",
                        equipmentId, alert.Message);
                }
            }
        }
    }
}
