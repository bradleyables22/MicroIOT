using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class Device:BaseModel
	{
		[Key]
		public long DeviceID { get; set; }

		public string Alias { get; set; } = string.Empty;

		public long? DeviceTypeID { get; set; }

		public long? DeviceGroupID { get; set; }

		public List<Entry>? Metadata { get; set; }

		public DateTime CreatedOn { get; set; }
		public DateTime? ConfirmedOn { get; set; }
		public DateTime? DeactivatedOn { get; set; }


		[ForeignKey(nameof(DeviceTypeID))]
		public DeviceType? DeviceType { get; set; }

		[ForeignKey(nameof(DeviceGroupID))]
		public DeviceGroup? DeviceGroup { get; set; }

		public ICollection<DeviceSensor>? Sensors { get; set; }
	}
}
