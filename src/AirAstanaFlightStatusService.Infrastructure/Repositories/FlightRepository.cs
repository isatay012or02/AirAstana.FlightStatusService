using AirAstanaFlightStatusService.Application.Common.Primitives;
using AirAstanaFlightStatusService.Application.DTO;
using AirAstanaFlightStatusService.Application.Interfaces.Repositories;
using AirAstanaFlightStatusService.Domain.Common.Enums;
using AirAstanaFlightStatusService.Domain.Entities;
using AirAstanaFlightStatusService.Infrastructure.Common.Exceptions;
using AirAstanaFlightStatusService.Infrastructure.Persistence;
using KDS.Primitives.FluentResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AirAstanaFlightStatusService.Infrastructure.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly DataContext _dataContext;
    private readonly ILogger<FlightRepository> _logger;

    public FlightRepository(DataContext dataContext, ILogger<FlightRepository> logger)
    {
        _dataContext = dataContext;
        _logger = logger;
    }
    
    public async Task<Result<List<Flight>>> GetFlightList(string origin, string destination, string userName)
    {
          
        List<Flight> result = null!;
        try
        {
            result = await _dataContext.Flights
                .Where(f => f.Origin == origin && f.Destination == destination)
                .OrderBy(f => f.Arrival)
                .ToListAsync();
            if (result.Count == 0)
            {
                _logger.LogError("{Message} {Action} {Date}", 
                    "Не удалось получить данные по запросу", nameof(GetFlightList), DateTime.Now);
                return Result.Failure<List<Flight>>(DomainError.NotFound);
            }
            
        }
        catch (DatabaseException ex)
        {
            _logger.LogError("{Message} {Action} {Date} {Exception}",
                "Ошибка при обращений на базу данных", nameof(GetFlightList), DateTime.Now, ex.Message);
            return Result.Failure<List<Flight>>(DomainError.DatabaseFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message} {Action} {Date} {Exception}",
                "Ошибка при обращений на базу данных", nameof(GetFlightList), DateTime.Now, ex.Message);
            return Result.Failure<List<Flight>>(DomainError.DatabaseFailed);
        }

        _logger.LogInformation("{Message} {Action}", "Данные успешно получены", nameof(GetFlightList));
        
        return Result.Success(result);
    }

    public async Task<Result> AddFlight(FlightDto request)
    {
        using (var transaction = _dataContext.Database.BeginTransaction())
        {
            try
            {
                await _dataContext.AddAsync(request);
                await _dataContext.SaveChangesAsync();

                transaction.Commit();
            }
            catch (DatabaseException ex)
            {
                transaction.Rollback();
                _logger.LogError("{Message} {Action} {UserName} {Date} {Exception}",
                    "Ошибка при обращений на базу данных", nameof(AddFlight), request.UserName, DateTime.Now, ex.Message);
                return Result.Failure(DomainError.DatabaseFailed);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError("{Message} {Action} {UserName} {Date} {Exception}",
                    "Не удалось добавить данные в базу даных", nameof(AddFlight), request.UserName, DateTime.Now, ex.Message);
                return Result.Failure(DomainError.DatabaseFailed);
            }
        }
        _logger.LogInformation("{Message} {Action} {UserName} {Date}", 
            "Данные успешно добавлены", nameof(AddFlight), request.UserName, DateTime.Now);

        return Result.Success();
    }

    public async Task<Result> UpdateFlight(int id, Status status, string userName)
    {
        using (var transaction = _dataContext.Database.BeginTransaction())
        {
            try
            {
                var updateData = await _dataContext.Flights
                    .Where(f => f.Id == id)
                    .FirstOrDefaultAsync();
                if (updateData is null)
                {
                    _logger.LogError("{Message} {Action} {UserName} {Date}",
                        "е удалось получить данные по запросу", nameof(UpdateFlight), userName, DateTime.Now);
                    return Result.Failure(DomainError.NotFound);   
                }

                updateData.Status = status;
                await _dataContext.SaveChangesAsync();

                transaction.Commit();
            }
            catch (DatabaseException ex)
            {
                transaction.Rollback();
                _logger.LogError("{Message} {Action} {UserName} {Date} {Exception}",
                    "Ошибка при обращений на базу данных", nameof(UpdateFlight), userName, DateTime.Now, ex.Message);
                return Result.Failure(DomainError.DatabaseFailed);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError("{Message} {Action} {UserName} {Date} {Exception}",
                    "Не удалось добавить данные в базу даных", nameof(UpdateFlight), userName, DateTime.Now, ex.Message);
                return Result.Failure(DomainError.DatabaseFailed);
            }
        }
        _logger.LogInformation("{Message} {Action} {UserName} {Date}", 
            "Данные успешно добавлены", nameof(UpdateFlight), userName, DateTime.Now);

        return Result.Success();
    }
}