using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Server.Data.DTOs.OtaOverride
{
	public class UpdateOtaOverrideDTO
	{
		[Description("The Device Group ID this override applies to, this is not a constraint to the device group table.")]
		public string DeviceGroupID { get; set; } = string.Empty;
		[Description("The Device ID this override applies to, this is not a constraint to the device table.")]
		public string DeviceID { get; set; } = string.Empty;
		[Description("The ID of the OTA manifest record this device should download.")]
		public long? OtaRecordID { get; set; }
		[Description("When the record was created.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("Any related notes for the override record.")]
		[MaxLength(500)]
		public string? Notes { get; set; }
	}
}
