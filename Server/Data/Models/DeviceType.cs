using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class DeviceType:BaseModel
	{
		[Key]
		[GraphQLDescription("The ID of the device type.")]
		[Description("The ID of the device type.")]
		public string DeviceTypeID { get; set; } = Guid.CreateVersion7().ToString();
		[GraphQLDescription("The name of the device type.")]
		[Description("The name of the device type.")]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		[GraphQLDescription("The optional description of the device type.")]
		[Description("The optional description of the device type.")]
		[MaxLength(500)]
		public string? Description { get; set; }
		[GraphQLDescription("Metadata associated with the device type.")]
		[Description("Metadata associated with the device type.")]
		public List<Entry>? Metadata { get; set; }
		[GraphQLDescription("When the device type was created.")]
		[Description("When the device type was created.")]
		public DateTime CreatedOn { get; set; }
		[GraphQLDescription("When the device type was deactivated, if applicable.")]
		[Description("When the device type was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[GraphQLIgnore]
		[JsonIgnore]
		public ICollection<Device>? Devices { get; set; }
	}
}
