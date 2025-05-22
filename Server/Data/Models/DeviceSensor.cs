using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Server.Data.Models
{
	public class DeviceSensor
	{
		[Key]
		public long SensorID { get; set; }

		public long DeviceID { get; set; }

		public long SensorTypeID { get; set; }

		public long SensorCategoryID { get; set; }

		public JsonDocument? Metadata { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime? DeactivatedOn { get; set; }

		[ForeignKey(nameof(DeviceID))]
		public Device Device { get; set; } = null!;

		[ForeignKey(nameof(SensorTypeID))]
		public SensorType SensorType { get; set; } = null!;

		[ForeignKey(nameof(SensorCategoryID))]
		public SensorCategory SensorCategory { get; set; } = null!;

		public ICollection<SensorReading>? Readings { get; set; }
	}
}
