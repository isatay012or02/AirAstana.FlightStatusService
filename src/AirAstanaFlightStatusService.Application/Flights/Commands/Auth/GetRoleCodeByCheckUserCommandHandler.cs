using AirAstanaFlightStatusService.Application.Interfaces.Repositories;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AirAstanaFlightStatusService.Application.Flights.Commands.Auth;

public class GetRoleCodeByCheckUserCommandHandler : IRequestHandler<GetRoleCodeByCheckUserCommand, Result<string>>
{
    private IAuthRepository _repository;

    public GetRoleCodeByCheckUserCommandHandler(IAuthRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<string>> Handle(GetRoleCodeByCheckUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetRoleCodeByCheckUser(request.UserName, request.Password);
        if (result.IsFailed)
        {
            
        }
        
        return result.Value;
    }
}