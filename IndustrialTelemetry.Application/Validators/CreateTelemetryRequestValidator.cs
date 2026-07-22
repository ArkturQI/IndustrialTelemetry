using FluentValidation;
using IndustrialTelemetry.Application.DTOs;

namespace IndustrialTelemetry.Application.Validators;

public class CreateTelemetryRequestValidator : AbstractValidator<CreateTelemetryRequest>
{
    public CreateTelemetryRequestValidator()
    {
        RuleFor(x => x.RequestId)
            .NotEmpty().WithMessage("RequestId is required.")
            .MaximumLength(100);

        RuleFor(x => x.EquipmentId)
            .NotEmpty().WithMessage("EquipmentId is required.");

        RuleFor(x => x.Value)
            .GreaterThanOrEqualTo(0).WithMessage("Value must be >= 0.");

        RuleFor(x => x.MetricType)
            .NotEmpty().WithMessage("MetricType is required.")
            .MaximumLength(50);

        RuleFor(x => x.Timestamp)
            .NotEmpty().WithMessage("Timestamp is required.");
    }
}