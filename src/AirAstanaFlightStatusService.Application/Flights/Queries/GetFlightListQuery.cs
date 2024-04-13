using AirAstanaFlightStatusService.Domain.Entities;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AirAstanaFlightStatusService.Application.Flights.Queries;

public class GetFlightListQuery : IRequest<Result<List<Flight>>>
{
    public GetFlightListQuery(DateTimeOffset arrival) => Arrival = arrival;
    
    public DateTimeOffset Arrival { get; set; }
}