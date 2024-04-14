using System.ComponentModel.DataAnnotations.Schema;

namespace AirAstanaFlightStatusService.Domain.Entities;

[Table(name: "Role", Schema = "public")]
public class Role
{
    public int Id { get; set; }
    public string? Code { get; set; }
}