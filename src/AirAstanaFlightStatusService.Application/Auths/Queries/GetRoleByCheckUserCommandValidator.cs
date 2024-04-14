using FluentValidation;

namespace AirAstanaFlightStatusService.Application.Auths.Queries;

public class GetRoleByCheckUserCommandValidator : AbstractValidator<GetRoleCodeByCheckUserCommand>
{
    public GetRoleByCheckUserCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName не может быть пустым");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password не может быть пустым");
    }
}