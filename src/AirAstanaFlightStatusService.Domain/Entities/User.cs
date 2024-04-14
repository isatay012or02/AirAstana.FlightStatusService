using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirAstanaFlightStatusService.Domain.Entities;

[Table(name: "User", Schema = "public")]
public class User
{
    [Key]
    public int Id { get; set; }
    
    public string? UserName { get; set; }
    
    public string? Password { get; set; }
    
    public int RoleId { get; set; }
}