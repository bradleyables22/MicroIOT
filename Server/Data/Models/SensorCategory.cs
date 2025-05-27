using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class SensorCategory:BaseModel
	{
		[Key]
		[Description("The sensor category ID.")]
		public long SensorCategoryID { get; set; }
		[Description("The name of the sensor category.")]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		[Description("The optional description of the sensor category, if applicable.")]
		[MaxLength(500)]
		public string? Description { get; set; }
		[Description("Metadata associated with this sensor category.")]
		public List<Entry>? Metadata { get; set; }
		[Description("When the sensor category was created.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When the sensor category was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[JsonIgnore]
		public ICollection<DeviceSensor>? DeviceSensors { get; set; }

	}
}
