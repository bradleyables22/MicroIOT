using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Server.Data.Models
{
	public class SystemGroup :BaseModel
	{
		[Key]
		[Description("The ID of the system group.")]
		public long SystemGroupID { get; set; }
		[Description("The name of the system group.")]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		[Description("A short description of the systemgroup.")]
		[MaxLength(500)]
		public string Description { get; set; } = string.Empty;
		[Description("User defined metadata related to this system group.")]
		public List<Entry>? Metadata { get; set; }
		[Description("When the documentation was created on.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When the documentation was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[Description("Documentation related to this system group.")]
		public ICollection<SystemGroupDocumentation>? Documentations { get; set; }
		[Description("Device groups belonging to this system group.")]
		public ICollection<DeviceGroup>? DeviceGroups { get; set; }
	}
}
