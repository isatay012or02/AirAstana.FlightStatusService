using KDS.Primitives.FluentResult;
using MediatR;

namespace AirAstanaFlightStatusService.Application.Auths.Queries;

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