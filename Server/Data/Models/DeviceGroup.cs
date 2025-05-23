using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class DeviceGroup:BaseModel
	{
		[Key]
		public long GroupID { get; set; }

		public string Name { get; set; } = string.Empty;

		public string? Description { get; set; }

		public long? SystemGroupID { get; set; }
		public long? GroupTypeID { get; set; }

		public JsonDocument? Metadata { get; set; }

		public DateTime CreatedOn { get; set; }
		public DateTime? ConfirmedOn { get; set; }
		public DateTime? DeactivatedOn { get; set; }

		[ForeignKey(nameof(GroupTypeID))]
		public GroupType? GroupType { get; set; }
		[JsonIgnore]
		[ForeignKey(nameof(SystemGroupID))]
		public SystemGroup? SystemGroup { get; set; }
		
		public ICollection<Device>? Devices { get; set; }
	}
}
