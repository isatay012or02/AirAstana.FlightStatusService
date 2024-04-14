using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using AirAstanaFlightStatusService.Application.DTO;
using AirAstanaFlightStatusService.Application.Flights.Commands;
using AirAstanaFlightStatusService.Application.Flights.Queries;
using AirAstanaFlightStatusService.Domain.Common.Enums;
using AirAstanaFlightStatusService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace AirAstanaFlightStatusService.Api.Controllers.v1;

/// <summary>
/// Предоставляет методы для работы с рейсами
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class FlightController : BaseController
{
    private readonly ILogger<FlightController> _logger;
    
    public FlightController(ILogger<FlightController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    /// <summary>
    /// Метод для получения списка рейсов 
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    [HttpGet("list")]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(List<Flight>))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.InternalServerError, type: typeof(ProblemDetails))]
    public async Task<IActionResult> GetFlightList(string origin, string destination)
    {
        var userName = GetTokenData();
        
        var result = await Mediator.Send(new GetFlightListQuery(origin, destination, userName));
        return result.IsFailed ? ProblemResponse(result.Error) : Ok(result.Value);
    }
    
    /// <summary>
    /// Метод для добавления рейсов
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize(Roles = "Moderator")]
    [HttpPost("")]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(List<Flight>))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.InternalServerError, type: typeof(ProblemDetails))]
    public async Task<IActionResult> AddFlight([FromBody] FlightDto request)
    {
        var userName = GetTokenData();
        
        var result = await Mediator.Send(new AddFlightCommand(request.Id, request.Origin, request.Distination, request.Departure, request.Arrival, request.Status, userName));
        return result.IsFailed ? ProblemResponse(result.Error) : Ok();
    }
    
    /// <summary>
    /// Метод для рекдактирования рейсов
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    [Authorize(Roles = "Moderator")]
    [HttpPut("")]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.OK, type: typeof(List<Flight>))]
    [ProducesResponseType(statusCode: (int)HttpStatusCode.InternalServerError, type: typeof(ProblemDetails))]
    public async Task<IActionResult> EditFlight(int id, Status status)
    {
        var userName = GetTokenData();
        
        var result = await Mediator.Send(new EditFlightCommand(id, status, userName));
        return result.IsFailed ? ProblemResponse(result.Error) : Ok();
    }

    private string GetTokenData()
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (authorizationHeader == null || !authorizationHeader.StartsWith("Bearer ")) 
            return string.Empty;
    
        var token = authorizationHeader.Substring("Bearer ".Length).Trim();
    
        var handler = new JwtSecurityTokenHandler();
        var tokenS = handler.ReadJwtToken(token);
    
        var claims = tokenS.Claims;
        var userName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
        var userRole = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
    
        return userName;
    }
}