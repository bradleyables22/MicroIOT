using Server.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Server.DTOs.Device
{
	public class CreateDeviceDTO
	{
		[Description("The ID of the device.")]
		public string DeviceID { get; set; } = Guid.CreateVersion7().ToString("N");
		[Description("The human readable alias of this device.")]
		[MaxLength(100)]
		public string Alias { get; set; } = string.Empty;
		[Description("The device type id of this device.")]
		public string? DeviceTypeID { get; set; }
		[Description("The parent device group id of this device.")]
		public string? DeviceGroupID { get; set; } = Guid.CreateVersion7().ToString("N");
		[Description("Metadata associated with device.")]
		public List<Entry>? Metadata { get; set; }
	}
}
