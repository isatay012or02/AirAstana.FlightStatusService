using AirAstanaFlightStatusService.Application.Interfaces.Persistence;
using AirAstanaFlightStatusService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirAstanaFlightStatusService.Infrastructure.Persistence;

public class DataContext : DbContext, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
    public DbSet<Flight> Flights { get; set; }

    public DbSet<User> Users { get; set; }
    
    public DbSet<Role> Roles { get; set; }
}