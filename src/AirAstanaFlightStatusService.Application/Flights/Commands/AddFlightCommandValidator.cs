using AirAstanaFlightStatusService.Application.Common.Validators;
using FluentValidation;

namespace AirAstanaFlightStatusService.Application.Flights.Commands;

public class AddFlightCommandValidator : AbstractValidator<AddFlightCommand>
{
    public AddFlightCommandValidator()
    {
        RuleFor(x => x.Origin).NotEmpty().WithMessage("Origin не может быть пустой");
        RuleFor(x => x.Destination).NotEmpty().WithMessage("Destination не может быть пустой");
        RuleFor(x => x.Departure.ToString()).SetValidator(new DateTimeOffsetValidator());
        RuleFor(x => x.Arrival.ToString()).SetValidator(new DateTimeOffsetValidator());
        RuleFor(x => x.Status).NotEmpty().WithMessage("Status не может быть пустым");
    }
}