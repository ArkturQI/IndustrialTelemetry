namespace IndustrialTelemetry.Domain.Models;

public class IdempotencyRecord
{
    protected IdempotencyRecord() { }

    public IdempotencyRecord(string requestId)
    {
        RequestId = requestId;
        ProcessedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string RequestId { get; private set; } = string.Empty;
    public DateTime ProcessedAt { get; private set; } = DateTime.UtcNow;
}