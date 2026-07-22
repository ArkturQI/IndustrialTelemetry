namespace IndustrialTelemetry.Application.Exceptions;

public class ConcurrencyException : Exception
{
    public ConcurrencyException()
        : base("The record was modified by another process. Please try again.")
    {
    }
}