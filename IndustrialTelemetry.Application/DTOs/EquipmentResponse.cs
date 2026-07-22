using IndustrialTelemetry.Domain.Enums;

namespace IndustrialTelemetry.Application.DTOs;

public class EquipmentResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public EquipmentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}