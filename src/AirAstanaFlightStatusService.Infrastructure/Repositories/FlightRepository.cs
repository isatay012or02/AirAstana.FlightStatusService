using AirAstanaFlightStatusService.Application.Common.Primitives;
using AirAstanaFlightStatusService.Application.DTO;
using AirAstanaFlightStatusService.Application.Interfaces.Repositories;
using AirAstanaFlightStatusService.Domain.Common.Enums;
using AirAstanaFlightStatusService.Domain.Entities;
using AirAstanaFlightStatusService.Infrastructure.Common.Exceptions;
using AirAstanaFlightStatusService.Infrastructure.Persistence;
using KDS.Primitives.FluentResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AirAstanaFlightStatusService.Infrastructure.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly DataContext _dataContext;
    private readonly ILogger<FlightRepository> _logger;
    private readonly IDistributedCache _cache;
    private readonly IConfiguration _configuration;

    public FlightRepository(DataContext dataContext, ILogger<FlightRepository> logger, IDistributedCache cache, IConfiguration configuration)
    {
        _dataContext = dataContext;
        _logger = logger;
        _cache = cache;
        _configuration = configuration;
    }
    
    public async Task<Result<List<Flight>>> GetFlightList(string origin, string destination, string userName)
    {
        List<Flight> result = null!;
        try
        {
            var cacheKey = _configuration["RedisOptions:GetFlightList"] + userName;
            var cacheFlightList = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cacheFlightList))
                return Result.Success(JsonConvert.DeserializeObject<List<Flight>>(cacheFlightList));
            
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

            var expirationTime = int.Parse(_configuration["RedisOptions:ExpirationInSecond"]);
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(result),
                new DistributedCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(expirationTime)});
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
        try
        {
            var model = new Flight
            {
                Origin = request.Origin,
                Destination = request.Destination,
                Arrival = request.Arrival,
                Departure = request.Departure,
                Status = request.Status
            };
                
            await _dataContext.AddAsync(model);
            await _dataContext.SaveChangesAsync();
        }
        catch (DatabaseException ex)
        {
            _logger.LogError("{Message} {Action} {UserName} {Date} {Exception}",
                "Ошибка при обращений на базу данных", nameof(AddFlight), request.UserName, DateTime.Now, ex.Message);
            return Result.Failure(DomainError.DatabaseFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message} {Action} {UserName} {Date} {Exception}",
                "Не удалось добавить данные в базу даных", nameof(AddFlight), request.UserName, DateTime.Now, ex.Message);
            return Result.Failure(DomainError.DatabaseFailed);
        }
            
        _logger.LogInformation("{Message} {Action} {UserName} {Date}", 
            "Данные успешно добавлены", nameof(AddFlight), request.UserName, DateTime.Now);

        return Result.Success();
    }

    public async Task<Result> UpdateFlight(int id, Status status, string userName)
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
        }
        catch (DatabaseException ex)
        {
            _logger.LogError("{Message} {Action} {UserName} {Date} {Exception}",
                "Ошибка при обращений на базу данных", nameof(UpdateFlight), userName, DateTime.Now, ex.Message);
            return Result.Failure(DomainError.DatabaseFailed);
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message} {Action} {UserName} {Date} {Exception}",
                "Не удалось добавить данные в базу даных", nameof(UpdateFlight), userName, DateTime.Now, ex.Message);
            return Result.Failure(DomainError.DatabaseFailed);
        }
        
        _logger.LogInformation("{Message} {Action} {UserName} {Date}", 
            "Данные успешно добавлены", nameof(UpdateFlight), userName, DateTime.Now);

        return Result.Success();
    }
}