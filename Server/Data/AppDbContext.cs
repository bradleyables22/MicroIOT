using Microsoft.EntityFrameworkCore;
using Server.Data.Models;

namespace Server.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		public DbSet<SystemGroup> SystemGroups => Set<SystemGroup>();
		public DbSet<SystemGroupDocumentation> SystemGroupDocumentations => Set<SystemGroupDocumentation>();
		public DbSet<DocumentationPage> DocumentationPages => Set<DocumentationPage>();
		public DbSet<DeviceType> DeviceTypes => Set<DeviceType>();
		public DbSet<SensorType> SensorTypes => Set<SensorType>();
		public DbSet<SensorCategory> SensorCategories => Set<SensorCategory>();
		public DbSet<GroupType> GroupTypes => Set<GroupType>();
		public DbSet<DeviceGroup> DeviceGroups => Set<DeviceGroup>();
		public DbSet<Device> Devices => Set<Device>();
		public DbSet<DeviceSensor> DeviceSensors => Set<DeviceSensor>();
		public DbSet<SensorReading> SensorReadings => Set<SensorReading>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<SystemGroup>()
				.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });

			modelBuilder.Entity<DeviceType>()
				.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });

			modelBuilder.Entity<SensorType>()
				.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });

			modelBuilder.Entity<SensorCategory>()
				.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });

			modelBuilder.Entity<GroupType>()
				.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });

			modelBuilder.Entity<DeviceGroup>()
				.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });

			modelBuilder.Entity<Device>()
				.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });

			modelBuilder.Entity<DeviceSensor>()
				.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });

			modelBuilder.Entity<SensorReading>()
				.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });

		}
	}
}
