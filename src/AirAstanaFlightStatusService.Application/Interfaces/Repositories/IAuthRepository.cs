using KDS.Primitives.FluentResult;

namespace AirAstanaFlightStatusService.Application.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<Result<string>> GetRoleCodeByCheckUser(string userName, string password);
}