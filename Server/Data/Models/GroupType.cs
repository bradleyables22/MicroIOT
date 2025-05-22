using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Server.Data.Models
{
	public class GroupType:BaseModel
	{
		[Key]
		public long GroupTypeID { get; set; }

		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public JsonDocument? Metadata { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime? DeactivatedOn { get; set; }

		public ICollection<DeviceGroup>? DeviceGroups { get; set; }
	}
}
