using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace Server.Data.Models
{
	public class DeviceGroup:BaseModel
	{
		[Key]
		[GraphQLDescription("The device group ID.")]
		[Description("The device group ID.")]
		public string GroupID { get; set; } = Guid.CreateVersion7().ToString("N");
		[GraphQLDescription("The given alias of this device group.")]
		[Description("The given alias of this device group.")]
		[MaxLength(100)]
		public string Alias { get; set; } = string.Empty;
		[GraphQLDescription("The system group this device belongs to.")]
		[Description("The system group this device belongs to.")]
		public long? SystemGroupID { get; set; }
		[GraphQLDescription("The device group type this device belongs to.")]
		[Description("The device group type this device belongs to.")]
		public long? GroupTypeID { get; set; }
		[GraphQLDescription("Metadata associated with this device group.")]
		[Description("The device group type this device belongs to.")]
		public List<Entry>? Metadata { get; set; }
		[GraphQLDescription("When this device group was created.")]
		[Description("The device group type this device belongs to.")]
		public DateTime CreatedOn { get; set; }
		[GraphQLDescription("When this device group confirmed it has received all system identifiers.")]
		[Description("When this device group confirmed it has received all system identifiers.")]
		public DateTime? ConfirmedOn { get; set; }
		[GraphQLDescription("When this device group was deactivated, if applicable.")]
		[Description("When this device group was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[Description("The device group type this device group belongs too.")]
		[GraphQLDescription("The device group type this device group belongs too.")]
		[ForeignKey(nameof(GroupTypeID))]
		public GroupType? GroupType { get; set; }
		[JsonIgnore]
		[GraphQLIgnore]
		[ForeignKey(nameof(SystemGroupID))]
		public SystemGroup? SystemGroup { get; set; }
		[GraphQLDescription("Child devices of this device group.")]
		[Description("Child devices of this device group.")]
		public ICollection<Device>? Devices { get; set; }
	}
}
