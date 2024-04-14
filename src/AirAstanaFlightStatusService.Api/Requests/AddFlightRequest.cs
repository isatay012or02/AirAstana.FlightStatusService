using AirAstanaFlightStatusService.Domain.Common.Enums;

namespace AirAstanaFlightStatusService.Api.Requests;

public record AddFlightRequest(string Origin, string Destination, DateTimeOffset Departure, DateTimeOffset Arrival, Status Status);