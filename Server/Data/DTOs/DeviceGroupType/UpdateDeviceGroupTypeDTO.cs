﻿using Server.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Server.DTOs.DeviceSensor
{
	public class UpdateDeviceGroupTypeDTO
	{
		[Description("The ID of the device group type.")]
		public long GroupTypeID { get; set; }
		[Description("The name of the device group type.")]
		[StringLength(100,MinimumLength =1)]
		public string Name { get; set; } = string.Empty;
		[Description("The optional description of the device group type.")]
        [StringLength(500, MinimumLength = 1)]
        public string? Description { get; set; }
		[Description("Metadata associated with this group type.")]
		public List<Entry>? Metadata { get; set; }
		[Description("When this group type was created.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When this group type was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }

	}
}
