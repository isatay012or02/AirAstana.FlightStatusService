using AirAstanaFlightStatusService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirAstanaFlightStatusService.Application.Interfaces.Persistence;

public interface IDataContext
{
    DbSet<Flight> Flights { get; set; }

    DbSet<User> Users { get; set; }
    
    DbSet<Role> Roles { get; set; }
}