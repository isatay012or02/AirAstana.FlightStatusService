using AirAstanaFlightStatusService.Application.Interfaces.Repositories;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AirAstanaFlightStatusService.Application.Flights.Commands;

public class EditFlightCommandHandler : IRequestHandler<EditFlightCommand, Result>
{
    private readonly IFlightRepository _repository;

    public EditFlightCommandHandler(IFlightRepository repository)
    {
        _repository = _repository;
    }
    
    public async Task<Result> Handle(EditFlightCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.UpdateFlight(request.Id, request.Status);
        if (result.IsFailed)
        {
            
        }

        return result;
    }
}