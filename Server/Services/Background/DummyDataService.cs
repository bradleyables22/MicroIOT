
using Server.Data;
using Server.Data.Models;

namespace Server.Services.Background
{
	public class DummyDataService : BackgroundService
	{
		private readonly IServiceScopeFactory _scopeFactory;

		public DummyDataService(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			using var scope = _scopeFactory.CreateScope();
			var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

			// Factory System
			var factorySystem = new SystemGroup
			{
				SystemGroupID = 1,
				Name = "Precision Metalworks Factory",
				Description = "Factory floor with CNC and Molding machines",
				Metadata = new List<Entry> { new() { Key = "Location", Value = "Detroit, MI" } }
			};
			_context.SystemGroups.Add(factorySystem);

			var cncGroupType = new DeviceGroupType { GroupTypeID = 1, Name = "CNC Zone" };
			var moldingGroupType = new DeviceGroupType { GroupTypeID = 2, Name = "Molding Zone" };
			_context.DeviceGroupTypes.AddRange(cncGroupType, moldingGroupType);

			var cncGroup = new DeviceGroup { DeviceGroupID = "CNC-GRP-1", Alias = "CNC Area", SystemGroupID = factorySystem.SystemGroupID, GroupTypeID = cncGroupType.GroupTypeID };
			var moldingGroup = new DeviceGroup { DeviceGroupID = "MOLD-GRP-1", Alias = "Molding Area", SystemGroupID = factorySystem.SystemGroupID, GroupTypeID = moldingGroupType.GroupTypeID };
			_context.DeviceGroups.AddRange(cncGroup, moldingGroup);

			var cnc = new DeviceType { DeviceTypeID = "CNC Machinery", Name = "CNC Machine" };
			var mold = new DeviceType { DeviceTypeID = "Moldering", Name = "Molder" };
			_context.DeviceTypes.AddRange(cnc, mold);

            var cncLathe = new Device { DeviceID = "CNC-001", Alias = "CNC Lathe", DeviceGroupID = cncGroup.DeviceGroupID , DeviceTypeID = cnc.DeviceTypeID};
			var injectionMolder = new Device { DeviceID = "MOLD-001", Alias = "Injection Molder", DeviceGroupID = moldingGroup.DeviceGroupID, DeviceTypeID = mold.DeviceTypeID };
			_context.Devices.AddRange(cncLathe, injectionMolder);

			var env = new SensorCategory { SensorCategoryID = 0, Name = "Enviromental" };
			_context.SensorCategories.AddRange(env);

			var tempType = new SensorType { SensorTypeID = "TEMP", Name = "Temperature Sensor" };
			var vibType = new SensorType { SensorTypeID = "VIB", Name = "Vibration Sensor" };
			_context.SensorTypes.AddRange(tempType, vibType);

			var tempReadingType = new ReadingType { ReadingTypeID = "TEMP", Name = "Temperature", Units = "C" };
			var vibReadingType = new ReadingType { ReadingTypeID = "VIB", Name = "Vibration", Units = "mm/s" };
			_context.ReadingTypes.AddRange(tempReadingType, vibReadingType);

			var latheTempSensor = new DeviceSensor { SensorID = "TEMP", DeviceID = cncLathe.DeviceID, SensorTypeID = tempType.SensorTypeID, SensorCategoryID = env.SensorCategoryID };
			var molderVibSensor = new DeviceSensor { SensorID = "VIB", DeviceID = injectionMolder.DeviceID, SensorTypeID = vibType.SensorTypeID, SensorCategoryID = env.SensorCategoryID };
			_context.DeviceSensors.AddRange(latheTempSensor, molderVibSensor);

			_context.SensorReadings.AddRange(new[]
			{
				new SensorReading { SensorID = latheTempSensor.SensorID, ReadingTypeID = tempReadingType.ReadingTypeID, Value = "68", Timestamp = DateTime.UtcNow },
				new SensorReading { SensorID = molderVibSensor.SensorID, ReadingTypeID = vibReadingType.ReadingTypeID, Value = "1.2", Timestamp = DateTime.UtcNow }
			});

			// Telematics System
			var fleetSystem = new SystemGroup
			{
				SystemGroupID = 2,
				Name = "Fleet Logistics Co.",
				Description = "GPS-enabled trailer monitoring system",
				Metadata = new List<Entry> { new() { Key = "FleetSize", Value = "200" } }
			};
			_context.SystemGroups.Add(fleetSystem);

			var trailerGroupType = new DeviceGroupType { GroupTypeID = 3, Name = "Dry Van Trailer" };
			_context.DeviceGroupTypes.Add(trailerGroupType);

            var gps = new DeviceType { DeviceTypeID = "GPS", Name = "Gps Device" };
            _context.DeviceTypes.AddRange(cnc, mold);

            var loc = new SensorCategory { SensorCategoryID = 0, Name = "Location" };
            _context.SensorCategories.AddRange(loc);

            var trailerGroup = new DeviceGroup { DeviceGroupID = "TRAILER-001", Alias = "Dry Van", SystemGroupID = fleetSystem.SystemGroupID, GroupTypeID = trailerGroupType.GroupTypeID };
			_context.DeviceGroups.Add(trailerGroup);

			var trailer1 = new Device { DeviceID = "TRAILER-001-GPS", Alias = "Trailer 001 GPS", DeviceGroupID = trailerGroup.DeviceGroupID, DeviceTypeID= gps.DeviceTypeID };
			_context.Devices.Add(trailer1);

			var gpsType = new SensorType { SensorTypeID = "NEO-M8N", Name = "NEO-M8N GPS Module" };
			_context.SensorTypes.Add(gpsType);

			var gpsReadingType = new ReadingType { ReadingTypeID = "GPS", Name = "GPS Location", Units = "Degrees" };
			_context.ReadingTypes.Add(gpsReadingType);

			var gpsSensor = new DeviceSensor { SensorID = "GPS-TRAILER-001", DeviceID = trailer1.DeviceID, SensorTypeID = gpsType.SensorTypeID, SensorCategoryID = loc.SensorCategoryID };
			_context.DeviceSensors.Add(gpsSensor);

			_context.SensorReadings.Add(new SensorReading
			{
				SensorID = gpsSensor.SensorID,
				ReadingTypeID = gpsReadingType.ReadingTypeID,
				Value = "37.7749,-122.4194",
				Timestamp = DateTime.UtcNow
			});

			await _context.SaveChangesAsync();
		}
	}
}