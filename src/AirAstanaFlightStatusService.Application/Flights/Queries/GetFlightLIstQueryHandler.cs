using AirAstanaFlightStatusService.Application.Interfaces.Persistence;
using AirAstanaFlightStatusService.Application.Interfaces.Repositories;
using AirAstanaFlightStatusService.Domain.Entities;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AirAstanaFlightStatusService.Application.Flights.Queries;

public class GetFlightLIstQueryHandler : IRequestHandler<GetFlightListQuery, Result<List<Flight>>>
{
    private readonly IFlightRepository _repository;
    
    public GetFlightLIstQueryHandler(IFlightRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<Flight>>> Handle(GetFlightListQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetFlightList(request.Arrival);
        if (result.IsFailed)
        {
            return null!;
        }

        return result;
    }
}