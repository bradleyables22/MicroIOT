using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Server.Data.Models
{
	public class DeviceGroup:BaseModel
	{
		[Key]
		public long GroupID { get; set; }

		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public long GroupTypeID { get; set; }

		public long SystemGroupID { get; set; }

		public JsonDocument? Metadata { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime? DeactivatedOn { get; set; }

		[ForeignKey(nameof(GroupTypeID))]
		public GroupType GroupType { get; set; } = null!;

		[ForeignKey(nameof(SystemGroupID))]
		public SystemGroup SystemGroup { get; set; } = null!;

		public ICollection<Device>? Devices { get; set; }
	}
}
