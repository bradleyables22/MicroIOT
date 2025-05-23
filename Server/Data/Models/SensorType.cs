using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class SensorType:BaseModel
	{
		[Key]
		public long SensorTypeID { get; set; }

		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public string Unit { get; set; } = string.Empty;

		public List<Entry>? Metadata { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime? DeactivatedOn { get; set; }
		[JsonIgnore]
		public ICollection<DeviceSensor>? DeviceSensors { get; set; }
	}
}
