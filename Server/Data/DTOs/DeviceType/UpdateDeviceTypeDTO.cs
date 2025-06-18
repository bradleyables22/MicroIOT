using Server.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Server.DTOs.DeviceType
{
	public class UpdateDeviceTypeDTO
	{
		[Description("The ID of the device type.")]
		public string DeviceTypeID { get; set; } = Guid.CreateVersion7().ToString();
		[Description("The name of the device type.")]
		[StringLength(100, MinimumLength = 1)]
		public string Name { get; set; } = string.Empty;
		[Description("The optional description of the device type.")]
		[StringLength(500, MinimumLength = 1)]
		public string? Description { get; set; }
		[Description("Metadata associated with the device type.")]
		public List<Entry>? Metadata { get; set; }
		[Description("When the device type was created.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When the device type was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
	}
}
