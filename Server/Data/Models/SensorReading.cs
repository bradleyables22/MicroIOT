using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Server.Data.Models
{
	public class SensorReading : BaseModel
	{
		[Key] // these needs to be here. but you need to composite key this for partitioning. create on sensorid+ partitionkey. setup part
		public long ReadingID { get; set; }

		public long? SensorID { get; set; }

		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
		[JsonIgnore]
		public int Year { get; set; } = DateTime.UtcNow.Year;
		[JsonIgnore]
		public byte Month { get; set; } = Convert.ToByte(DateTime.UtcNow.Month);
		[JsonIgnore]
		public byte Day { get; set; } = Convert.ToByte(DateTime.UtcNow.Day);
		public int PartitionKey => Year * 100 + Month;
		public string Value { get; set; } = string.Empty;

		public JsonDocument? Metadata { get; set; }

		public DateTime? DeactivatedOn { get; set; }
		[JsonIgnore]
		[ForeignKey(nameof(SensorID))]
		public DeviceSensor? Sensor { get; set; }
	}
}
