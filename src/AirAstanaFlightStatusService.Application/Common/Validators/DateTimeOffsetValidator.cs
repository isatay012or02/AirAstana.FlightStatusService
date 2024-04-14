using FluentValidation;

namespace AirAstanaFlightStatusService.Application.Common.Validators;

public class DateTimeOffsetValidator : AbstractValidator<string>
{
    public DateTimeOffsetValidator()
    {
        RuleFor(x => x).NotEmpty().WithMessage("Дата не может быть пустой")
            .Matches(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}[+-]\d{2}:\d{2}$")
            .WithMessage("Неверный формат DateTimeOffset");
    }
}