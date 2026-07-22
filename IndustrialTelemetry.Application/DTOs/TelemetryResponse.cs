namespace IndustrialTelemetry.Application.DTOs;

public class TelemetryResponse
{
    public Guid Id { get; set; }
    public Guid EquipmentId { get; set; }
    public string RequestId { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string MetricType { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}