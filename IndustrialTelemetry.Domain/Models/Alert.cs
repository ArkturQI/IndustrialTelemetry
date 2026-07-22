namespace IndustrialTelemetry.Domain.Models;

public class Alert
{
    protected Alert() { }

    public Alert(Guid equipmentId, string message)
    {
        EquipmentId = equipmentId;
        Message = message;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid EquipmentId { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public Equipment Equipment { get; private set; } = null!;
}