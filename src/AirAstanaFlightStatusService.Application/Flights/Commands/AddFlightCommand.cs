using AirAstanaFlightStatusService.Domain.Common.Enums;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AirAstanaFlightStatusService.Application.Flights.Commands;

public class AddFlightCommand : IRequest<Result>
{
    public AddFlightCommand(string origin, string destination, DateTimeOffset departure, DateTimeOffset arrival, Status status, string userName)
    {
        Origin = origin;
        Destination = destination;
        Departure = departure;
        Arrival = arrival;
        Status = status;
        UserName = userName;
    }
    
    public string Origin { get; private set; }
    public string Destination { get; private set; }
    public DateTimeOffset Departure { get; private set; }
    public DateTimeOffset Arrival { get; private set; }
    public Status Status { get; private set; }
    public string UserName { get; set; }
}