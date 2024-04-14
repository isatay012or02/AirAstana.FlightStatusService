using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirAstanaFlightStatusService.Domain.Entities;

[Table(name: "User", Schema = "public")]
public class User : Entity
{
    [Column(name:"username")]
    public string? UserName { get; set; }
    
    [Column(name:"password")]
    public string? Password { get; set; }
    
    [Column(name:"roleid")]
    public int RoleId { get; set; }
}