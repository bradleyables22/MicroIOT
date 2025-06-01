using Server.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Server.DTOs.SystemGroup
{
	public class CreateSystemGroupDTO
	{
		[Description("The name of the system group.")]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		[Description("A short description of the systemgroup.")]
		[MaxLength(500)]
		public string Description { get; set; } = string.Empty;
		[Description("User defined metadata related to this system group.")]
		public List<Entry>? Metadata { get; set; }
	}
}
