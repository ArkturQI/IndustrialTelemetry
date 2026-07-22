namespace IndustrialTelemetry.Application.Exceptions;

public class EquipmentNotFoundException : Exception
{
    public EquipmentNotFoundException(Guid equipmentId)
        : base($"Equipment with ID {equipmentId} not found.")
    {
    }
}