using FluentValidation;

namespace AirAstanaFlightStatusService.Application.Flights.Commands;

public class EditFlightCommandValidator : AbstractValidator<EditFlightCommand>
{
    public EditFlightCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id не может быть пустым");
        RuleFor(x => x.Status).NotEmpty().WithMessage("Status не может быть пустым");
    }
}