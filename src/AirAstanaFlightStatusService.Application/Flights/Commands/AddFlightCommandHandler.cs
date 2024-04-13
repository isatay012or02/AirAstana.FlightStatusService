using AirAstanaFlightStatusService.Application.DTO;
using AirAstanaFlightStatusService.Application.Interfaces.Repositories;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AirAstanaFlightStatusService.Application.Flights.Commands;

public class AddFlightCommandHandler : IRequestHandler<AddFlightCommand, Result>
{
    private readonly IFlightRepository _repository;
    
    public AddFlightCommandHandler(IFlightRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Result> Handle(AddFlightCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.AddFlight(
            new FlightDto(request.Id, request.Origin, request.Destination, request.Departure, request.Arrival, request.Status));
        if (result.IsFailed)
        {
                
        }
        
        return result;
    }
}