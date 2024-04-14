using AirAstanaFlightStatusService.Domain.Common.Enums;

namespace AirAstanaFlightStatusService.Application.DTO;

public record FlightDto(string Origin, string Destination, DateTimeOffset Departure, DateTimeOffset Arrival, Status Status, string UserName);