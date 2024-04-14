using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirAstanaFlightStatusService.Domain.Common.Enums;

namespace AirAstanaFlightStatusService.Domain.Entities;

[Table(name: "Flight", Schema = "public")]
public class Flight : Entity
{
    [Column(name:"origin")]
    public string? Origin { get; set; }
    
    [Column(name:"destination")]
    public string? Destination { get; set; }
    
    [Column(name:"departure")]
    public DateTimeOffset Departure { get; set; }
    
    [Column(name:"arrival")]
    public DateTimeOffset Arrival { get; set; }
    
    [Column(name:"status")]
    public Status Status { get; set; }
}