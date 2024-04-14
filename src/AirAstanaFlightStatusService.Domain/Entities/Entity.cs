using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirAstanaFlightStatusService.Domain.Entities;

public class Entity
{
    [Key]
    [Column(name:"id")]
    public int Id { get; set; }
}