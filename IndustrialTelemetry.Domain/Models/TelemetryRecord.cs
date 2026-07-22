namespace IndustrialTelemetry.Domain.Models;

public class TelemetryRecord
{
    protected TelemetryRecord() { }

    public TelemetryRecord(Guid equipmentId, string requestId, decimal value, string metricType, DateTime timestamp)
    {
        EquipmentId = equipmentId;
        RequestId = requestId;
        Value = value;
        MetricType = metricType;
        Timestamp = timestamp;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid EquipmentId { get; private set; }
    public string RequestId { get; private set; } = string.Empty;
    public decimal Value { get; private set; }
    public string MetricType { get; private set; } = string.Empty;
    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

    public Equipment Equipment { get; private set; } = null!;
}