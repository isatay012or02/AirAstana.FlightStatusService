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
    
    public async Task<Result<List<Flight>>> GetFlightList(DateTimeOffset arrival)
    {
        List<Flight> result = null!;
        
        try
        {
            result = await _dataContext.Flights
                .Where(f => f.Arrival == arrival)
                .ToListAsync();
            
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ошибка при обращений на базу данных. Описание ошибки: {ex.Message}");
        }
        
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
                _logger.LogError($"Не удалось добавить данные, ошибка на уровне базы данных. Описание: {ex.Message}");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError($"Не удалось добавить данные в базу даных. Описание ошибки: {ex.Message}");
            }
        }

        return Result.Success();
    }

    public async Task<Result> UpdateFlight(int id, Status status)
    {
        using (var transaction = _dataContext.Database.BeginTransaction())
        {
            try
            {
                var updateData = await _dataContext.Flights
                    .Where(f => f.Id == id)
                    .FirstOrDefaultAsync();
                if (updateData is null)
                    return Result.Success();

                updateData.Status = status;
                await _dataContext.SaveChangesAsync();

                transaction.Commit();
            }
            catch (DatabaseException ex)
            {
                transaction.Rollback();
                _logger.LogError($"Не удалось добавить данные, ошибка на уровне базы данных. Описание: {ex.Message}");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError($"Не удалось добавить данные в базу даных. Описание ошибки: {ex.Message}");
            }
        }

        return Result.Success();
    }
}