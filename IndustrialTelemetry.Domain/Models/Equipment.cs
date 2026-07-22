using IndustrialTelemetry.Domain.Enums;

namespace IndustrialTelemetry.Domain.Models;

public class Equipment
{
    protected Equipment() { }

    public Equipment(string name)
    {
        Name = name;
        Status = EquipmentStatus.Normal;
        CreatedAt = DateTime.UtcNow;
    }

    public Equipment(Guid id, string name)
    {
        Id = id;
        Name = name;
        Status = EquipmentStatus.Normal;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public EquipmentStatus Status { get; private set; } = EquipmentStatus.Normal;
    public uint Version { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public ICollection<TelemetryRecord> TelemetryRecords { get; private set; } = new List<TelemetryRecord>();
    public ICollection<Alert> Alerts { get; private set; } = new List<Alert>();

    public void UpdateStatus(EquipmentStatus newStatus)
    {
        Status = newStatus;
    }
}