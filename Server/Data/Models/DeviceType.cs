using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Server.Data.Models
{
	public class DeviceType:BaseModel
	{
		[Key]
		public long DeviceTypeID { get; set; }

		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public JsonDocument? Metadata { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime? DeactivatedOn { get; set; }

		public ICollection<Device>? Devices { get; set; }
	}
}
