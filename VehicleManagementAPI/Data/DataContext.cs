using Microsoft.EntityFrameworkCore;
using VehicleManagementAPI.Models;

namespace VehicleManagementAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleOwner> VehicleOwners { get; set; }
    }
}
