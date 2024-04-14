using AirAstanaFlightStatusService.Domain.Common.Enums;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AirAstanaFlightStatusService.Application.Flights.Commands;

public class EditFlightCommand : IRequest<Result>
{
    public EditFlightCommand(int id, Status status, string userName)
    {
        Id = id;
        Status = status;
        UserName = userName;
    }
    
    public int Id { get; private set; }
    
    public Status Status { get; private set; }
    
    public string UserName { get; set; }
}