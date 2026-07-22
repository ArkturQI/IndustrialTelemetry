using IndustrialTelemetry.Application.DTOs;
using IndustrialTelemetry.Application.Exceptions;
using IndustrialTelemetry.Application.Interfaces;
using IndustrialTelemetry.Domain.Models;
using IndustrialTelemetry.Infrastructure.DATA;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IndustrialTelemetry.Application.Services;

public class EquipmentService : IEquipmentService
{
    private readonly TelemetryDbContext _dbContext;
    private readonly ILogger<EquipmentService> _logger;

    public EquipmentService(TelemetryDbContext dbContext, ILogger<EquipmentService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<EquipmentResponse> GetEquipmentByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var equipment = await _dbContext.Equipments
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (equipment == null)
        {
            throw new EquipmentNotFoundException(id);
        }

        return MapToResponse(equipment);
    }

    public async Task<EquipmentResponse> UpdateEquipmentStatusAsync(
        Guid id,
        UpdateEquipmentStatusRequest request,
        CancellationToken cancellationToken)
    {
        var equipment = await _dbContext.Equipments
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (equipment == null)
        {
            throw new EquipmentNotFoundException(id);
        }

    
        equipment.UpdateStatus(request.NewStatus);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Equipment status updated successfully. Id: {Id}, NewStatus: {Status}", id, request.NewStatus);
        }
        catch (DbUpdateConcurrencyException ex)
        {
       
            _logger.LogWarning(ex, "Concurrency conflict detected when updating equipment {Id}", id);
            throw new ConcurrencyException();
        }

        return MapToResponse(equipment);
    }

    private static EquipmentResponse MapToResponse(Equipment equipment)
    {
        return new EquipmentResponse
        {
            Id = equipment.Id,
            Name = equipment.Name,
            Status = equipment.Status,
            CreatedAt = equipment.CreatedAt
        };
    }
}