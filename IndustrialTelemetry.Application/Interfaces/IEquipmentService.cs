using IndustrialTelemetry.Application.DTOs;

namespace IndustrialTelemetry.Application.Interfaces;

public interface IEquipmentService
{
    Task<EquipmentResponse> GetEquipmentByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<EquipmentResponse> UpdateEquipmentStatusAsync(Guid id, UpdateEquipmentStatusRequest request, CancellationToken cancellationToken);
}