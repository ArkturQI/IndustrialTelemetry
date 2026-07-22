namespace IndustrialTelemetry.Application.DTOs;

public class CreateTelemetryRequest
{
    public string RequestId { get; set; } = string.Empty;
    public Guid EquipmentId { get; set; }
    public decimal Value { get; set; }
    public string MetricType { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}