using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class SensorType:BaseModel
	{
		[Key]
		[GraphQLDescription("The ID of the sensor type.")]
		[Description("The ID of the sensor type.")]
		public string SensorTypeID { get; set; } = Guid.CreateVersion7().ToString();
		[GraphQLDescription("The the name of the sensor type.")]
		[Description("The the name of the sensor type.")]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		[GraphQLDescription("The description of the sensor type.")]
		[Description("The optional description of the sensor type, if applicable.")]
		[MaxLength(500)]
		public string? Description { get; set; }
		[GraphQLDescription("Metadata associated with these sensor types.")]
		[Description("Metadata associated with these sensor types.")]
		public List<Entry>? Metadata { get; set; }
		[GraphQLDescription("When this sensor type was created.")]
		[Description("When this sensor type was created.")]
		public DateTime CreatedOn { get; set; }
		[GraphQLDescription("When this sensor type was deactivated, if applicable.")]
		[Description("When this sensor type was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[JsonIgnore]
		[GraphQLIgnore]
		public ICollection<DeviceSensor>? DeviceSensors { get; set; }
	}
}
