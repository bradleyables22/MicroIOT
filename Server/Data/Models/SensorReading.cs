using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Server.Data.Models
{
	public class SensorReading : BaseModel
	{
		[Key]
		public long ReadingID { get; set; }

		public long SensorID { get; set; }

		public DateTime Timestamp { get; set; }

		public string Value { get; set; } = string.Empty;

		public JsonDocument? Metadata { get; set; }

		public DateTime? DeactivatedOn { get; set; }

		[ForeignKey(nameof(SensorID))]
		public DeviceSensor Sensor { get; set; } = null!;
	}
}
