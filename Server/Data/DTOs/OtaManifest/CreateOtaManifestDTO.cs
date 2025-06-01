using Server.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Server.Data.DTOs.OtaManifest
{
	public class CreateOtaManifestDTO
	{
		[Description("The ID of the device type this record is associated with.")]
		public string? DeviceTypeID { get; set; }

		[Description("The version of the OTA firmware.")]
		[MaxLength(100)]
		public string Version { get; set; } = string.Empty;
		[Description("Where they can download the firmware from.")]
		public string Url { get; set; } = string.Empty;
		[Description("Any related notes for the record.")]
		[MaxLength(500)]
		public string? Notes { get; set; }
	}
}
