using Microsoft.EntityFrameworkCore;
using Server.Data.Models;

namespace Server.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		#region Sets
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
		#endregion


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			#region MetaData Setup
			//modelBuilder.Entity<SystemGroup>()
			//	.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });
			//modelBuilder.Entity<DeviceType>()
			//	.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });

			//modelBuilder.Entity<SensorType>()
			//	.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });

			//modelBuilder.Entity<SensorCategory>()
			//	.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });

			//modelBuilder.Entity<GroupType>()
			//	.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });

			//modelBuilder.Entity<DeviceGroup>()
			//	.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });

			//modelBuilder.Entity<Device>()
			//	.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });

			//modelBuilder.Entity<DeviceSensor>()
			//	.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });

			//modelBuilder.Entity<SensorReading>()
			//	.OwnsOne(_ => _.Metadata, b => { b.ToJson(); });
			#endregion

			#region Deletion Policies
			modelBuilder.Entity<Device>()
				.HasMany(_=>_.Sensors).WithOne(_=>_.Device).HasForeignKey(_=>_.DeviceID).OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<SensorCategory>()
				.HasMany(_ => _.DeviceSensors).WithOne(_ => _.SensorCategory).HasForeignKey(_ => _.SensorCategoryID).OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<SensorType>()
				.HasMany(_ => _.DeviceSensors).WithOne(_ => _.SensorType).HasForeignKey(_ => _.SensorTypeID).OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<DeviceSensor>()
				.HasMany(_ => _.Readings).WithOne(_ => _.Sensor).HasForeignKey(_ => _.SensorID).OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<DeviceType>()
				.HasMany(_ => _.Devices).WithOne(_ => _.DeviceType).HasForeignKey(_ => _.DeviceTypeID).OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<DeviceGroup>()
				.HasMany(_ => _.Devices).WithOne(_ => _.DeviceGroup).HasForeignKey(_ => _.DeviceGroupID).OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<SystemGroup>()
				.HasMany(_ => _.Documentations).WithOne(_ => _.SystemGroup).HasForeignKey(_ => _.SystemGroupID).OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<SystemGroupDocumentation>()
				.HasMany(_ => _.Pages).WithOne(_ => _.Document).HasForeignKey(_ => _.DocumentID).OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<SystemGroup>()
				.HasMany(_ => _.DeviceGroups).WithOne(_ => _.SystemGroup).HasForeignKey(_ => _.SystemGroupID).OnDelete(DeleteBehavior.SetNull);

			#endregion
			#region Indexes
			//modelBuilder.Entity<SensorReading>()
			//	.HasIndex(_ => _.Year);
			//modelBuilder.Entity<SensorReading>()
			//	.HasIndex(_ => _.Month);
			//modelBuilder.Entity<SensorReading>()
			//	.HasIndex(_ => _.Day);
			#endregion
		}
	}
}
