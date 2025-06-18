using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Server.Data.DTOs.OtaManifest
{
	public class UpdateOtaManifestDTO
	{
		[Description("The ID of the manefest record.")]
		public long RecordID { get; set; }
		[Description("The ID of the device type this record is associated with.")]
		public string? DeviceTypeID { get; set; }

		[Description("The version of the OTA firmware.")]
        [StringLength(100, MinimumLength = 1)]
        public string Version { get; set; } = string.Empty;
		[Description("Where they can download the firmware from.")]
        [StringLength(700, MinimumLength = 1)]
        public string Url { get; set; } = string.Empty;
		[Description("Any related notes for the record.")]
		[MaxLength(500)]
		public string? Notes { get; set; }
	}
}
