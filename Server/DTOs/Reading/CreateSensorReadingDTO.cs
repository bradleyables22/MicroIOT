using Server.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Server.DTOs.Reading
{
	public class CreateSensorReadingDTO
	{
		[Description("The sensor ID this reading is associated with.")]
		public string? SensorID { get; set; }
		[Description("The reading type ID associated with this reading.")]
		public string? ReadingTypeID { get; set; }
		[Description("The recorded value of this reading.")]
		public string Value { get; set; } = string.Empty;
		[Description("Metadata associated with this sensor reading.")]
		public List<Entry>? Metadata { get; set; }
	}
}
