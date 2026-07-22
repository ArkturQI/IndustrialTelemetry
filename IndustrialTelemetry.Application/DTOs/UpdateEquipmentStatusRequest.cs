using IndustrialTelemetry.Domain.Enums;

namespace IndustrialTelemetry.Application.DTOs;

public class UpdateEquipmentStatusRequest
{
    public EquipmentStatus NewStatus { get; set; }
}