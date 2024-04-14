using FluentValidation;

namespace AirAstanaFlightStatusService.Application.Flights.Queries;

public class GetFlightListQueryValidator : AbstractValidator<GetFlightListQuery>
{
    public GetFlightListQueryValidator()
    {
        RuleFor(x => x.Origin).NotEmpty().WithMessage("Origin не может быть пустым");
        RuleFor(x => x.Destination).NotEmpty().WithMessage("Destination не может быть пустым");
    }
}