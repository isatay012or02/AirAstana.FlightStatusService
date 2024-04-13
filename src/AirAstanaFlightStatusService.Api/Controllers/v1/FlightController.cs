using Microsoft.AspNetCore.Mvc;
using System.Net;
using AirAstanaFlightStatusService.Application.DTO;
using AirAstanaFlightStatusService.Application.Flights.Commands;
using AirAstanaFlightStatusService.Application.Flights.Queries;
using AirAstanaFlightStatusService.Domain.Common.Enums;
using AirAstanaFlightStatusService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace AirAstanaFlightStatusService.Api.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class FlightController : BaseController
{
    private readonly ILogger<FlightController> _logger;
    
    public FlightController(ILogger<FlightController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    [HttpGet("list")]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(List<Flight>))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.InternalServerError, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetFlightList(DateTimeOffset arrival)
    {
        var result = await Mediator.Send(new GetFlightListQuery(arrival));
        return result.IsFailed ? ProblemResponse(result.Error) : Ok(result.Value);
    }
    
    [Authorize(Roles = "Moderator")]
    [HttpPost("")]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(List<Flight>))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.InternalServerError, type: typeof(ProblemDetails))]
    public async Task<IActionResult> AddFlight([FromBody] FlightDto request)
    {
        var result = await Mediator.Send(new AddFlightCommand(request.Id, request.Origin, request.Distination, request.Departure, request.Arrival, request.Status));
        return result.IsFailed ? ProblemResponse(result.Error) : Ok();
    }
    
    [Authorize(Roles = "Moderator")]
    [HttpPut("")]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(List<Flight>))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.InternalServerError, type: typeof(ProblemDetails))]
    public async Task<IActionResult> EditFlight(int id, Status status)
    {
        var result = await Mediator.Send(new EditFlightCommand(id, status));
        return result.IsFailed ? ProblemResponse(result.Error) : Ok();
    }
}