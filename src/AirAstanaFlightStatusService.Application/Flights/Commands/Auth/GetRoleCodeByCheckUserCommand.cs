using KDS.Primitives.FluentResult;
using MediatR;

namespace AirAstanaFlightStatusService.Application.Flights.Commands.Auth;

public class GetRoleCodeByCheckUserCommand : IRequest<Result<string>>
{
    public string UserName { get; private set; }
    public string Password { get; private set; }
    
    public GetRoleCodeByCheckUserCommand(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }   
}