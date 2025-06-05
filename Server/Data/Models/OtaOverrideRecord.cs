using Server.Data.DTOs.OtaOverride;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Server.Data.Models
{
	public class OtaOverrideRecord :BaseModel
	{
		public OtaOverrideRecord()
		{
			
		}

		public OtaOverrideRecord(CreateOtaOverrideDTO create)
		{
			RecordID = $"{create.DeviceGroupID}|{create.DeviceID}";
			DeviceGroupID = create.DeviceGroupID;
			DeviceID = create.DeviceID;
			OtaRecordID = create.OtaRecordID;
			Notes = create.Notes;
			CreatedOn = DateTime.UtcNow;
		}

		public OtaOverrideRecord(UpdateOtaOverrideDTO update)
		{
			RecordID = $"{update.DeviceGroupID}|{update.DeviceID}";
			DeviceGroupID = update.DeviceGroupID;
			DeviceID = update.DeviceID;
			OtaRecordID= update.OtaRecordID;
			CreatedOn= update.CreatedOn;
			Notes = update.Notes;
		}

		[Key]
		[Description("The Override record ID.")]
		public string RecordID { get; set; } = string.Empty;
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
		[Description("The OTA manifest record this device should download.")]
		[ForeignKey(nameof(OtaRecordID))]
		public OtaManifestRecord? Record { get; set; }

	}
}
