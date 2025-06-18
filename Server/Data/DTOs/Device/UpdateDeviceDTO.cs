using Server.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Server.DTOs.Device
{
	public class UpdateDeviceDTO
	{
		[Description("The ID of the device.")]
		public string DeviceID { get; set; } = Guid.CreateVersion7().ToString("N");
		[Description("The human readable alias of this device.")]
		[StringLength(100, MinimumLength = 1)]
		public string Alias { get; set; } = string.Empty;
		[Description("The device type id of this device.")]
		public string? DeviceTypeID { get; set; }
		[Description("The parent device group id of this device.")]
		public string? DeviceGroupID { get; set; } = Guid.CreateVersion7().ToString("N");
		[Description("Metadata associated with device.")]
		public List<Entry>? Metadata { get; set; }
		[Description("When this device was created.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When this device was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
	}
}
