﻿using Microsoft.EntityFrameworkCore;
using Server.Data.Models;
using Server.Extensions;
using System.Text.Json;

namespace Server.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		#region Sets
		public DbSet<SystemGroup> SystemGroups { get; set; }
		public DbSet<SystemGroupDocumentation> SystemGroupDocumentations { get; set; }
		public DbSet<DocumentationPage> DocumentationPages { get; set; }
		public DbSet<DeviceType> DeviceTypes { get; set; }
		public DbSet<SensorType> SensorTypes { get; set; }
		public DbSet<SensorCategory> SensorCategories { get; set; }
		public DbSet<GroupType> GroupTypes { get; set; }
		public DbSet<DeviceGroup> DeviceGroups { get; set; }
		public DbSet<Device> Devices { get; set; }
		public DbSet<DeviceSensor> DeviceSensors { get; set; }
		public DbSet<SensorReading> SensorReadings { get; set; }
		#endregion


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			#region MetaData Setup
			// SystemGroup
			modelBuilder.Entity<SystemGroup>()
				.Property(e => e.Metadata)
				.HasConversion(JsonValueConverter.ListOfEntryConverter);

			// DeviceType
			modelBuilder.Entity<DeviceType>()
				.Property(e => e.Metadata)
				.HasConversion(JsonValueConverter.ListOfEntryConverter); 

			// SensorType
			modelBuilder.Entity<SensorType>()
				.Property(e => e.Metadata)
				.HasConversion(JsonValueConverter.ListOfEntryConverter); 

			// SensorCategory
			modelBuilder.Entity<SensorCategory>()
				.Property(e => e.Metadata)
				.HasConversion(JsonValueConverter.ListOfEntryConverter); 

			// GroupType
			modelBuilder.Entity<GroupType>()
				.Property(e => e.Metadata)
				.HasConversion(JsonValueConverter.ListOfEntryConverter);

			// DeviceGroup
			modelBuilder.Entity<DeviceGroup>()
				.Property(e => e.Metadata)
				.HasConversion(JsonValueConverter.ListOfEntryConverter);

			// Device
			modelBuilder.Entity<Device>()
				.Property(e => e.Metadata)
				.HasConversion(JsonValueConverter.ListOfEntryConverter);

			// DeviceSensor
			modelBuilder.Entity<DeviceSensor>()
				.Property(e => e.Metadata)
				.HasConversion(JsonValueConverter.ListOfEntryConverter);

			// SensorReading
			modelBuilder.Entity<SensorReading>()
				.Property(e => e.Metadata)
				.HasConversion(JsonValueConverter.ListOfEntryConverter);
			#endregion

			#region Deletion Policies
			modelBuilder.Entity<Device>()
				.HasMany(_ => _.Sensors).WithOne(_ => _.Device).HasForeignKey(_ => _.DeviceID).OnDelete(DeleteBehavior.Cascade);

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
