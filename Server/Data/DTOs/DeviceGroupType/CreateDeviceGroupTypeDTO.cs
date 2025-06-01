using Server.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Server.DTOs.DeviceSensor
{
	public class CreateDeviceGroupTypeDTO
	{
		[Description("The name of the device group type.")]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		[Description("The optional description of the device group type.")]
		[MaxLength(500)]
		public string? Description { get; set; }
		[Description("Metadata associated with this group type.")]
		public List<Entry>? Metadata { get; set; }
	}
}
