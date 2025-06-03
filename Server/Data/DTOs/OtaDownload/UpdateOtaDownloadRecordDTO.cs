using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Server.Data.DTOs.OtaDownload
{
	public class UpdateOtaDownloadRecordDTO
	{
		[Description("The ID of the manifest record.")]
		public long RecordID { get; set; }
		[Description("The Device ID this override applies to, this is not a constraint to the device table.")]
		public string DeviceID { get; set; } = string.Empty;
		[Description("The version of the OTA firmware.")]
		[MaxLength(100)]
		public string Version { get; set; } = string.Empty;
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
	}
}
