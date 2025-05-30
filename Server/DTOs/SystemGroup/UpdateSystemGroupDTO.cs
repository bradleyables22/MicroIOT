using Server.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Server.DTOs.SystemGroup
{
	public class UpdateSystemGroupDTO
	{
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
		[Description("When the system group was created.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When the system group was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
	}
}
