using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Server.Data.Models
{
	public class Device:BaseModel
	{
		[Key]
		public long DeviceID { get; set; }

		public string Name { get; set; } = string.Empty;

		public long DeviceTypeID { get; set; }

		public long DeviceGroupID { get; set; }

		public JsonDocument? Metadata { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime? DeactivatedOn { get; set; }

		[ForeignKey(nameof(DeviceTypeID))]
		public DeviceType DeviceType { get; set; } = null!;

		[ForeignKey(nameof(DeviceGroupID))]
		public DeviceGroup DeviceGroup { get; set; } = null!;

		public ICollection<DeviceSensor>? Sensors { get; set; }
	}
}
