using System.ComponentModel.DataAnnotations.Schema;

namespace AirAstanaFlightStatusService.Domain.Entities;

[Table(name: "Role", Schema = "public")]
public class Role : Entity
{
    [Column(name:"code")]
    public string? Code { get; set; }
}