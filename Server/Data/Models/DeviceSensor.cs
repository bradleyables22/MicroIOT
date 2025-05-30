using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel;
using Server.DTOs.DeviceSensor;

namespace Server.Data.Models
{
	public class DeviceSensor :BaseModel
	{
		public DeviceSensor()
		{
			
		}

		public DeviceSensor(CreateDeviceSensorDTO create)
		{
			SensorID = create.SensorID;
			DeviceID = create.DeviceID;
			SensorTypeID = create.SensorTypeID;
			Metadata = create.Metadata;
			CreatedOn = DateTime.UtcNow;
		}

		public DeviceSensor(UpdateDeviceSensorDTO update)
		{
			SensorID = update.SensorID;
			DeviceID = update.DeviceID;
			SensorTypeID = update.SensorTypeID;
			Metadata = update.Metadata;
			CreatedOn = update.CreatedOn;
			DeactivatedOn = update.DeactivatedOn;
		}

		[Key]
		[Description("The ID of this sensor.")]
		public string SensorID { get; set; }= Guid.CreateVersion7().ToString("N");
		[Description("The ID of the device this sensor is associated with.")]
		public string? DeviceID { get; set; }
		[Description("The ID of the sensor type of this sensor.")]
		public string? SensorTypeID { get; set; }
		[Description("The sensor category of this sensor.")]
		public long? SensorCategoryID { get; set; }
		[Description("Metadata associated with this sensor.")]
		public List<Entry>? Metadata { get; set; }
		[Description("When this sensor was created.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When this sensor was deactivated on, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }

		[JsonIgnore]
		[ForeignKey(nameof(DeviceID))]
		public Device? Device { get; set; }

		[Description("The sensor type of this sensor.")]
		[ForeignKey(nameof(SensorTypeID))]
		public SensorType? SensorType { get; set; }
		[Description("The sensor category of this sensor.")]
		[ForeignKey(nameof(SensorCategoryID))]
		public SensorCategory? SensorCategory { get; set; }
		[Description("Historical reading of this sensor.")]
		public ICollection<SensorReading>? Readings { get; set; }
	}
}
