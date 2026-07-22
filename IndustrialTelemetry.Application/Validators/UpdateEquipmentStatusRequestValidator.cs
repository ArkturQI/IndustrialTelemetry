using FluentValidation;
using IndustrialTelemetry.Application.DTOs;

namespace IndustrialTelemetry.Application.Validators;

public class UpdateEquipmentStatusRequestValidator : AbstractValidator<UpdateEquipmentStatusRequest>
{
    public UpdateEquipmentStatusRequestValidator()
    {
        RuleFor(x => x.NewStatus)
            .IsInEnum().WithMessage("Invalid status value.");
    }
}