using AirAstanaFlightStatusService.Application.DTO;
using AirAstanaFlightStatusService.Application.Interfaces.Repositories;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AirAstanaFlightStatusService.Application.Flights.Commands;

public class AddFlightCommandHandler : IRequestHandler<AddFlightCommand, Result>
{
    private readonly IFlightRepository _repository;
    private readonly ILogger<AddFlightCommandHandler> _logger;
    
    public AddFlightCommandHandler(IFlightRepository repository, ILogger<AddFlightCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Result> Handle(AddFlightCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.AddFlight(
            new FlightDto(request.Origin, request.Destination, request.Departure, request.Arrival, request.Status, request.UserName));
        
        return result;
    }
}