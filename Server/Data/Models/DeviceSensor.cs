using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace Server.Data.Models
{
	public class DeviceSensor :BaseModel
	{
		[Key]
		[GraphQLDescription("The ID of this sensor.")]
		[Description("The ID of this sensor.")]
		public string SensorID { get; set; }= Guid.CreateVersion7().ToString("N");
		[GraphQLDescription("The ID of the device this sensor is associated with.")]
		[Description("The ID of the device this sensor is associated with.")]
		public string? DeviceID { get; set; }
		[GraphQLDescription("The ID of the sensor type of this sensor.")]
		[Description("The ID of the sensor type of this sensor.")]
		public string? SensorTypeID { get; set; }
		[GraphQLDescription("The sensor category of this sensor.")]
		[Description("The sensor category of this sensor.")]
		public long? SensorCategoryID { get; set; }
		[GraphQLDescription("Metadata associated with this sensor.")]
		[Description("Metadata associated with this sensor.")]
		public List<Entry>? Metadata { get; set; }
		[GraphQLDescription("When this sensor was created.")]
		[Description("When this sensor was created.")]
		public DateTime CreatedOn { get; set; }
		[GraphQLDescription("When this sensor was deactivated on, if applicable.")]
		[Description("When this sensor was deactivated on, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }

		[GraphQLIgnore]
		[JsonIgnore]
		[ForeignKey(nameof(DeviceID))]
		public Device? Device { get; set; }

		[GraphQLDescription("The sensor type of this sensor.")]
		[Description("The sensor type of this sensor.")]
		[ForeignKey(nameof(SensorTypeID))]
		public SensorType? SensorType { get; set; }
		[Description("The sensor category of this sensor.")]
		[GraphQLDescription("The sensor category of this sensor.")]
		[ForeignKey(nameof(SensorCategoryID))]
		public SensorCategory? SensorCategory { get; set; }
		[GraphQLDescription("Historical reading of this sensor.")]
		[Description("Historical reading of this sensor.")]
		public ICollection<SensorReading>? Readings { get; set; }
	}
}
