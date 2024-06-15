using DeviceManagementSystem.Core.Entities;
using DeviceManagementSystem.Utilities.Connections;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementSystem.Infrastructure.Data
{
    public class DeviceContext : DbContext
    {
        public DeviceContext(DbContextOptions<DeviceContext> options) : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>()
                .Property(d => d.DeviceType)
                .HasConversion<string>();

            modelBuilder.Entity<Device>()
                .Property(d => d.Status)
                .HasConversion<string>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = ConnectionStringHelper.GetCustomConnectionString("DeviceDatabase");
                optionsBuilder.UseSqlServer("connectionString");
                base.OnConfiguring(optionsBuilder);
            }
        }
    }
}
