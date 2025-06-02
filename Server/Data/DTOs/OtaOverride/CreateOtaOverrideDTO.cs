﻿using Server.Data.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Data.DTOs.OtaOverride
{
	public class CreateOtaOverrideDTO
	{
		[Description("The Device ID this override applies to, this is not a constraint to the device table.")]
		public string DeviceID { get; set; } = string.Empty;
		[Description("The ID of the OTA manifest record this device should download.")]
		public long? OtaRecordID { get; set; }
		[Description("Any related notes for the override record.")]
		[MaxLength(500)]
		public string? Notes { get; set; }
	}
}
