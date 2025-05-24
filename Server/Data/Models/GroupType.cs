using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class GroupType:BaseModel
	{
		[Key]
		[GraphQLDescription("The ID of the device group type.")]
		[Description("The ID of the device group type.")]
		public long GroupTypeID { get; set; }
		[GraphQLDescription("The name of the device group type.")]
		[Description("The name of the device group type.")]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		[GraphQLDescription("The optional description of the device group type.")]
		[Description("The optional description of the device group type.")]
		[MaxLength(500)]
		public string? Description { get; set; }
		[GraphQLDescription("Metadata associated with this group type.")]
		[Description("Metadata associated with this group type.")]
		public List<Entry>? Metadata { get; set; }
		[GraphQLDescription("When this group type was created.")]
		[Description("When this group type was created.")]
		public DateTime CreatedOn { get; set; }
		[GraphQLDescription("When this group type was deactivated, if applicable.")]
		[Description("When this group type was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[GraphQLIgnore]
		[JsonIgnore]
		public ICollection<DeviceGroup>? DeviceGroups { get; set; }
	}
}
