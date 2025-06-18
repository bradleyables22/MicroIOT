using Server.Data.DTOs.OtaManifest;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class OtaManifestRecord:BaseModel
	{

		public OtaManifestRecord()
		{
			
		}

		public OtaManifestRecord(CreateOtaManifestDTO create)
		{
			RecordID = 0;
			DeviceTypeID = create.DeviceTypeID;
			Version = create.Version;
			Url = create.Url;
			CreatedOn = DateTime.UtcNow;
			Notes = create.Notes;
		}

		[Key]
		[Description("The ID of the manefest record.")]
		public long RecordID { get; set; }
		[Description("The ID of the device type this record is associated with.")]
		public string? DeviceTypeID { get; set; }

		[Description("The version of the OTA firmware.")]
		[MaxLength(100)]
		public string Version { get; set; }= string.Empty;
		[Description("Where they can download the firmware from.")]
		[MaxLength(700)]
		public string Url { get; set; } = string.Empty;
		[Description("When the record was created.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("Whether this record should be used as a default for this device type.")]
		public bool Default { get; set; } = false;
		public DateTime? DeactivatedOn { get; set; }
		[Description("Any related notes for the record.")]
		[MaxLength(500)]
		public string? Notes { get; set; }
		[JsonIgnore]
		[Description("The Devicetype this firmware is related too.")]
		[ForeignKey(nameof(DeviceTypeID))]
		public DeviceType? DeviceType { get; set; }
		[JsonIgnore]
		public ICollection<OtaOverrideRecord>? Overrides { get; set; }
	}
}
