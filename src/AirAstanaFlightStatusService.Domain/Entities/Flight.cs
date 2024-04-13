using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirAstanaFlightStatusService.Domain.Common.Enums;

namespace AirAstanaFlightStatusService.Domain.Entities;

[Table(name: "Flight", Schema = "public")]
public class Flight
{
    [Key]
    public int Id { get; set; }
    
    public string? Origin { get; set; }
    
    public string? Destination { get; set; }
    
    public DateTimeOffset Departure { get; set; }
    
    public DateTimeOffset Arrival { get; set; }
    
    public Status Status { get; set; }
}