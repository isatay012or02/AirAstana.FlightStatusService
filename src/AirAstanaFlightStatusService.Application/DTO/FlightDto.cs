using AirAstanaFlightStatusService.Domain.Common.Enums;

namespace AirAstanaFlightStatusService.Application.DTO;

public record FlightDto(int Id, string Origin, string Distination, DateTimeOffset Departure, DateTimeOffset Arrival, Status Status, string UserName);