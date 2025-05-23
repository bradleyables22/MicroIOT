using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class DeviceSensor :BaseModel
	{
		[Key]
		public long SensorID { get; set; }

		public long? DeviceID { get; set; }

		public long? SensorTypeID { get; set; }

		public long? SensorCategoryID { get; set; }

		public List<Entry>? Metadata { get; set; }

		public DateTime CreatedOn { get; set; }
		public DateTime? ConfirmedOn { get; set; }
		public DateTime? DeactivatedOn { get; set; }

		[JsonIgnore]
		[ForeignKey(nameof(DeviceID))]
		public Device? Device { get; set; }

		[ForeignKey(nameof(SensorTypeID))]
		public SensorType? SensorType { get; set; }

		[ForeignKey(nameof(SensorCategoryID))]
		public SensorCategory? SensorCategory { get; set; }

		public ICollection<SensorReading>? Readings { get; set; }
	}
}
