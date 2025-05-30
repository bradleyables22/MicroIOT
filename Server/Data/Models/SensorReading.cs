using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel;
using Server.DTOs.Reading;

namespace Server.Data.Models
{
	public class SensorReading : BaseModel
	{
		public SensorReading()
		{
			
		}

		public SensorReading(CreateSensorReadingDTO create)
		{
			DateTime dt = DateTime.UtcNow;

			ReadingID = 0;
			SensorID = create.SensorID;
			ReadingTypeID = create.SensorID;
			Timestamp = dt;
			Year = dt.Year;
			Month = Convert.ToByte(dt.Month);
			Day = Convert.ToByte(dt.Day);
			Value = create.Value;
			Metadata = create.Metadata;

		}

		[Key] // these needs to be here. but you need to composite key this for partitioning. create on sensorid+ partitionkey. setup part
		[Description("The ID of this reading.")]
		public long ReadingID { get; set; }
		[Description("The sensor ID this reading is associated with.")]
		public string? SensorID { get; set; }
		[Description("The reading type ID associated with this reading.")]
		public string? ReadingTypeID { get; set; }
		[Description("When this reading was recorded.")]
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
		[Description("The year this reading was recorded.")]
		public int Year { get; set; } = DateTime.UtcNow.Year;
		[Description("The month this reading was recorded.")]
		public byte Month { get; set; } = Convert.ToByte(DateTime.UtcNow.Month);
		[Description("The day this reading was recorded.")]
		public byte Day { get; set; } = Convert.ToByte(DateTime.UtcNow.Day);
		[JsonIgnore]
		public int PartitionKey => Year * 100 + Month;
		[Description("The recorded value of this reading.")]
		public string Value { get; set; } = string.Empty;
		[Description("Metadata associated with this sensor reading.")]
		public List<Entry>? Metadata { get; set; }
		[JsonIgnore]
		[ForeignKey(nameof(SensorID))]
		public DeviceSensor? Sensor { get; set; }

		[Description("The type of reading")]
		[ForeignKey(nameof(ReadingTypeID))]
		public ReadingType? ReadingType { get; set; }
	}
}
