using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class SensorCategory:BaseModel
	{
		[Key]
		[GraphQLDescription("The sensor category ID.")]
		[Description("The sensor category ID.")]
		public long SensorCategoryID { get; set; }
		[GraphQLDescription("The name of the sensor category.")]
		[Description("The name of the sensor category.")]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		[GraphQLDescription("The optional description of the sensor category.")]
		[Description("The optional description of the sensor category, if applicable.")]
		[MaxLength(500)]
		public string? Description { get; set; }
		[GraphQLDescription("Metadata associated with this sensor category.")]
		[Description("Metadata associated with this sensor category.")]
		public List<Entry>? Metadata { get; set; }
		[GraphQLDescription("When the sensor category was created.")]
		[Description("When the sensor category was created.")]
		public DateTime CreatedOn { get; set; }
		[GraphQLDescription("When the sensor category was deactivated, if applicable.")]
		[Description("When the sensor category was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[GraphQLIgnore]
		[JsonIgnore]
		public ICollection<DeviceSensor>? DeviceSensors { get; set; }

	}
}
