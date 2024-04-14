using AirAstanaFlightStatusService.Domain.Entities;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AirAstanaFlightStatusService.Application.Flights.Queries;

public class GetFlightListQuery : IRequest<Result<List<Flight>>>
{
    public GetFlightListQuery(string origin, string destination, string userName)
    {
        Origin = origin;
        Destination = destination;
        UserName = userName;
    }

    public string Origin { get;  set; }
    public string Destination { get; set; }
    public string UserName { get; set; }
}