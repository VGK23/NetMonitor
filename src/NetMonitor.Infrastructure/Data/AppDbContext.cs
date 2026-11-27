using Microsoft.EntityFrameworkCore;
using NetMonitor.Domain.Entities;
using NetMonitor.Domain.Enums;

namespace NetMonitor.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<DeviceGroup> DeviceGroups => Set<DeviceGroup>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<CheckLog> CheckLogs => Set<CheckLog>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<DeviceGroup>(e =>
        {
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
        });

        b.Entity<Device>(e =>
        {
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.Property(x => x.IdAddress).HasMaxLength(45).IsRequired();
            e.Property(x => x.Type).HasConversion<string>().HasMaxLength(20); 
            e.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
            e.HasOne(x => x.Group).WithMany(g => g.Devices).HasForeignKey(x => x.GroupId).OnDelete(DeleteBehavior.SetNull);
        });

        b.Entity<CheckLog>(e =>
        {
            e.HasOne(x => x.Device).WithMany(d => d.CheckLogs).HasForeignKey(x => x.DeviceId).OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(x => new {x.DeviceId, x.CheckedAt});
        });

        b.Entity<User>(e =>
        {
            e.HasIndex(x => x.UserName).IsUnique();
            e.Property(x => x.UserName).HasMaxLength(50).IsRequired();
            e.Property(x => x.Role).HasConversion<string>().HasMaxLength(20);
        });

        b.Entity<DeviceGroup>().HasData(new DeviceGroup {Id = 1, Name = "Phòng server"});

        b.Entity<Device>().HasData(
            new Device {Id = 1, Name = "Router chính", IdAddress = "192.168.1.1", Type = DeviceType.Router, GroupId = 1, Location = "Tầng 1"},
            new Device {Id = 2, Name = "Switch tầng 1", IdAddress = "192.168.1.2", Type = DeviceType.Switch, GroupId = 1, Location = "Tầng 1"},
            new Device {Id = 3, Name = "Web Server", IdAddress = "192.168.1.10", Port = 80, Type = DeviceType.Server, GroupId = 1, Location = "Phòng Server"}
            );
    }
}