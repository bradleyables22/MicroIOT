using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Server.Data.Models
{
	public class SystemGroup :BaseModel
	{
		[Key]
		[GraphQLDescription("The ID of the system group.")]
		[Description("The ID of the system group.")]
		public long SystemGroupID { get; set; }
		[GraphQLDescription("The name of the system group.")]
		[Description("The name of the system group.")]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		[GraphQLDescription("A short description of the systemgroup.")]
		[Description("A short description of the systemgroup.")]
		[MaxLength(500)]
		public string Description { get; set; } = string.Empty;
		[GraphQLDescription("User defined metadata related to this system group.")]
		[Description("User defined metadata related to this system group.")]
		public List<Entry>? Metadata { get; set; }
		[GraphQLDescription("When the documentation was created on.")]
		[Description("When the documentation was created on.")]
		public DateTime CreatedOn { get; set; }
		[GraphQLDescription("When the documentation was deactivated, if it is deactivated.")]
		[Description("When the documentation was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[GraphQLDescription("Documentation related to this system group.")]
		[Description("Documentation related to this system group.")]
		public ICollection<SystemGroupDocumentation>? Documentations { get; set; }
		[GraphQLDescription("Device groups belonging to this system group.")]
		[Description("Device groups belonging to this system group.")]
		public ICollection<DeviceGroup>? DeviceGroups { get; set; }
	}
}
