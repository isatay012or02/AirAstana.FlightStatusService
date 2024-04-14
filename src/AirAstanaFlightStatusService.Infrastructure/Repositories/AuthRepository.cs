using AirAstanaFlightStatusService.Application.Common.Primitives;
using AirAstanaFlightStatusService.Application.Interfaces.Repositories;
using AirAstanaFlightStatusService.Infrastructure.Common.Exceptions;
using AirAstanaFlightStatusService.Infrastructure.Persistence;
using KDS.Primitives.FluentResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AirAstanaFlightStatusService.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly DataContext _dataContext;
    private readonly ILogger<FlightRepository> _logger;

    public AuthRepository(DataContext dataContext, ILogger<FlightRepository> logger)
    {
        _dataContext = dataContext;
        _logger = logger;
    }

    public async Task<Result<string>> GetRoleCodeByCheckUser(string userName, string password)
    {
        string? roleCode;
        try
        {
            roleCode = await _dataContext.Users
                .Where(u => u.UserName == userName && u.Password == password)
                .Join(_dataContext.Roles,
                    user => user.RoleId,
                    role => role.Id,
                    (user, role) => role.Code)
                .FirstOrDefaultAsync();
            if (roleCode == string.Empty)
            {
                _logger.LogError($"Не удалось получить данные по запросу.");
                return Result.Failure<string>(DomainError.NotFound);
            }
        }
        catch (DatabaseException ex)
        {
            _logger.LogError($"Ошибка на уровне базы данных. Описание: {ex.Message}");
            return Result.Failure<string>(DomainError.DatabaseFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка при обращений на базу данных. Описание ошибки: {ex.Message}");
            return Result.Failure<string>(DomainError.DatabaseFailed);
        }

        return Result.Success(roleCode!);
    }
}