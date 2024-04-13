using AirAstanaFlightStatusService.Application.DTO;
using AirAstanaFlightStatusService.Domain.Common.Enums;
using AirAstanaFlightStatusService.Domain.Entities;
using KDS.Primitives.FluentResult;

namespace AirAstanaFlightStatusService.Application.Interfaces.Repositories;

public interface IFlightRepository
{
    Task<Result<List<Flight>>> GetFlightList(DateTimeOffset arrival);
    Task<Result> AddFlight(FlightDto request);
    Task<Result> UpdateFlight(int id, Status status);
}