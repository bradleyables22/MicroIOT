using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace Server.Data.Models
{
	public class SensorReading : BaseModel
	{
		[Key] // these needs to be here. but you need to composite key this for partitioning. create on sensorid+ partitionkey. setup part
		[GraphQLDescription("The ID of this reading.")]
		[Description("The ID of this reading.")]
		public long ReadingID { get; set; }
		[GraphQLDescription("The sensor ID this reading is associated with.")]
		[Description("The sensor ID this reading is associated with.")]
		public string? SensorID { get; set; }
		[GraphQLDescription("The reading type ID associated with this reading.")]
		[Description("The reading type ID associated with this reading.")]
		public string? ReadingTypeID { get; set; }
		[GraphQLDescription("When this reading was recorded.")]
		[Description("When this reading was recorded.")]
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
		[GraphQLDescription("The year this reading was recorded.")]
		[Description("The year this reading was recorded.")]
		public int Year { get; set; } = DateTime.UtcNow.Year;
		[GraphQLDescription("The month this reading was recorded.")]
		[Description("The month this reading was recorded.")]
		public byte Month { get; set; } = Convert.ToByte(DateTime.UtcNow.Month);
		[GraphQLDescription("The day this reading was recorded.")]
		[Description("The day this reading was recorded.")]
		public byte Day { get; set; } = Convert.ToByte(DateTime.UtcNow.Day);
		[GraphQLIgnore]
		[JsonIgnore]
		public int PartitionKey => Year * 100 + Month;
		[GraphQLDescription("The recorded value of this reading.")]
		[Description("The recorded value of this reading.")]
		public string Value { get; set; } = string.Empty;
		[GraphQLDescription("Metadata associated with this sensor reading.")]
		[Description("Metadata associated with this sensor reading.")]
		public List<Entry>? Metadata { get; set; }
		[JsonIgnore]
		[GraphQLIgnore]
		[ForeignKey(nameof(SensorID))]
		public DeviceSensor? Sensor { get; set; }
		[GraphQLDescription("The type of reading")]
		[Description("The type of reading")]
		[ForeignKey(nameof(ReadingTypeID))]
		public ReadingType? ReadingType { get; set; }
	}
}
