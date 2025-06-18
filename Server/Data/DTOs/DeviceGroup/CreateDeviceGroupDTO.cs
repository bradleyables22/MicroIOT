using Server.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Server.DTOs.DeviceGroup
{
	public class CreateDeviceGroupDTO
	{
		[Description("The device group ID.")]
		public string DeviceGroupID { get; set; } = Guid.CreateVersion7().ToString("N");
		[Description("The given alias of this device group.")]
		[StringLength(100, MinimumLength = 1)]
		public string Alias { get; set; } = string.Empty;
		[Description("The system group this device belongs to.")]
		public long? SystemGroupID { get; set; }
		[Description("The device group type this device belongs to.")]
		public long? DeviceGroupTypeID { get; set; }
		[Description("The device group type this device belongs to.")]
		public List<Entry>? Metadata { get; set; }
	}
}
