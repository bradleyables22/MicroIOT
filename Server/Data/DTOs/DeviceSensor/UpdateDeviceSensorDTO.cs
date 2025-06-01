using Server.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Server.DTOs.DeviceSensor
{
	public class UpdateDeviceSensorDTO
	{
		[Description("The ID of this sensor.")]
		public string SensorID { get; set; } = Guid.CreateVersion7().ToString("N");
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
	}
}
