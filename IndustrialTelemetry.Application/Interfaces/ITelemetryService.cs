using IndustrialTelemetry.Application.DTOs;

namespace IndustrialTelemetry.Application.Interfaces;

public interface ITelemetryService
{
    Task<TelemetryResponse> CreateTelemetryAsync(CreateTelemetryRequest request, CancellationToken cancellationToken);
    Task<List<TelemetryResponse>> GetTelemetryByEquipmentIdAsync(Guid equipmentId, CancellationToken cancellationToken);
}