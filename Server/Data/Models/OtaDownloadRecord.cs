using Server.Data.DTOs.OtaDownload;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Server.Data.Models
{
	public class OtaDownloadRecord :BaseModel
	{
		public OtaDownloadRecord()
		{
			
		}

		public OtaDownloadRecord(CreateOtaDownloadRecordDTO create)
		{
			var dt = DateTime.UtcNow;

			RecordID = 0;
			DeviceID = create.DeviceID;
			Version = create.Version;
			CreatedOn = dt;
			Year = dt.Year;
			Month = Convert.ToByte(dt.Month);
			Day = Convert.ToByte(dt.Day);
		}

		public OtaDownloadRecord(UpdateOtaDownloadRecordDTO update)
		{
			RecordID = update.RecordID;
			DeviceID = update.DeviceID;
			Version = update.Version;
			CreatedOn = update.CreatedOn;
			Year = update.CreatedOn.Year;
			Month = Convert.ToByte(update.CreatedOn.Month);
			Day = Convert.ToByte(update.CreatedOn.Day);
		}

		[Key]
		[Description("The ID of the manifest record.")]
		public long RecordID { get; set; }
		[Description("The Device ID this override applies to, this is not a constraint to the device table.")]
		public string DeviceID { get; set; } = string.Empty;
		[Description("The version of the OTA firmware.")]
		[MaxLength(100)]
		public string Version { get; set; } = string.Empty;
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("The year this download was recorded.")]
		public int Year { get; set; } = DateTime.UtcNow.Year;
		[Description("The month this download was recorded.")]
		public byte Month { get; set; } = Convert.ToByte(DateTime.UtcNow.Month);
		[Description("The day this download was recorded.")]
		public byte Day { get; set; } = Convert.ToByte(DateTime.UtcNow.Day);
		[JsonIgnore]
		public int PartitionKey => Year * 100 + Month;
	}
}
