using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class DeviceGroupType :BaseModel
	{
		[Key]
		[Description("The ID of the device group type.")]
		public long GroupTypeID { get; set; }
		[Description("The name of the device group type.")]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		[Description("The optional description of the device group type.")]
		[MaxLength(500)]
		public string? Description { get; set; }
		[Description("Metadata associated with this group type.")]
		public List<Entry>? Metadata { get; set; }
		[Description("When this group type was created.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When this group type was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[JsonIgnore]
		public ICollection<DeviceGroup>? DeviceGroups { get; set; }
	}
}
