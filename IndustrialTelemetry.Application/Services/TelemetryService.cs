using IndustrialTelemetry.Application.DTOs;
using IndustrialTelemetry.Application.Interfaces;
using IndustrialTelemetry.Domain.Models;
using IndustrialTelemetry.Infrastructure.DATA;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IndustrialTelemetry.Application.Services;

public class TelemetryService : ITelemetryService
{
    private readonly TelemetryDbContext _dbContext;
    private readonly ILogger<TelemetryService> _logger;

    public TelemetryService(TelemetryDbContext dbContext, ILogger<TelemetryService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<TelemetryResponse> CreateTelemetryAsync(
        CreateTelemetryRequest request,
        CancellationToken cancellationToken)
    {
        var existingRecord = await _dbContext.IdempotencyRecords
            .FirstOrDefaultAsync(x => x.RequestId == request.RequestId, cancellationToken);

        if (existingRecord != null)
        {
            _logger.LogWarning("Duplicate RequestId detected: {RequestId}. Returning existing record.", request.RequestId);

            var existingTelemetry = await _dbContext.TelemetryRecords
                .FirstOrDefaultAsync(t => t.RequestId == request.RequestId, cancellationToken);

            return MapToResponse(existingTelemetry!);
        }

        var equipment = await _dbContext.Equipments
            .FirstOrDefaultAsync(e => e.Id == request.EquipmentId, cancellationToken);

        if (equipment == null)
        {
            equipment = new Equipment(request.EquipmentId, $"Auto-Equipment-{request.EquipmentId}");
            _dbContext.Equipments.Add(equipment);
        }

        var telemetryRecord = new TelemetryRecord(
            request.EquipmentId,
            request.RequestId,
            request.Value,
            request.MetricType,
            request.Timestamp
        );

        var idempotencyRecord = new IdempotencyRecord(request.RequestId);

        _dbContext.TelemetryRecords.Add(telemetryRecord);
        _dbContext.IdempotencyRecords.Add(idempotencyRecord);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Telemetry record created successfully. RequestId: {RequestId}", request.RequestId);
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("duplicate key") == true)
        {
            _logger.LogWarning("Duplicate RequestId caught by database constraint: {RequestId}", request.RequestId);
            throw;
        }

        return MapToResponse(telemetryRecord);
    }

    public async Task<List<TelemetryResponse>> GetTelemetryByEquipmentIdAsync(
        Guid equipmentId,
        CancellationToken cancellationToken)
    {
        var records = await _dbContext.TelemetryRecords
            .Where(t => t.EquipmentId == equipmentId)
            .OrderByDescending(t => t.Timestamp)
            .Take(100)
            .ToListAsync(cancellationToken);

        return records.Select(MapToResponse).ToList();
    }

    private static TelemetryResponse MapToResponse(TelemetryRecord record)
    {
        return new TelemetryResponse
        {
            Id = record.Id,
            EquipmentId = record.EquipmentId,
            RequestId = record.RequestId,
            Value = record.Value,
            MetricType = record.MetricType,
            Timestamp = record.Timestamp
        };
    }
}