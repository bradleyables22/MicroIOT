using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Server.Data.Models
{
	public class Device:BaseModel
	{
		[Key]
		[GraphQLDescription("The ID of the device.")]
		[Description("The ID of the device.")]
		public string DeviceID { get; set; } = Guid.CreateVersion7().ToString("N");
		[GraphQLDescription("The human readable alias of this device.")]
		[Description("The human readable alias of this device.")]
		[MaxLength(100)]
		public string Alias { get; set; } = string.Empty;
		[GraphQLDescription("The device type id of this device.")]
		[Description("The device type id of this device.")]
		public string? DeviceTypeID { get; set; }
		[GraphQLDescription("The parent device group id of this device.")]
		[Description("The parent device group id of this device.")]
		public string? DeviceGroupID { get; set; }=Guid.CreateVersion7().ToString("N");
		[Description("Metadata associated with device.")]
		[GraphQLDescription("Metadata associated with device.")]
		public List<Entry>? Metadata { get; set; }
		[GraphQLDescription("When this device was created.")]
		[Description("When this device was created.")]
		public DateTime CreatedOn { get; set; }
		[GraphQLDescription("When this device was deactivated, if applicable.")]
		[Description("When this device was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[Description("The device type of this device.")]
		[GraphQLDescription("The device type of this device.")]
		[ForeignKey(nameof(DeviceTypeID))]
		public DeviceType? DeviceType { get; set; }
		[GraphQLIgnore]
		[ForeignKey(nameof(DeviceGroupID))]
		public DeviceGroup? DeviceGroup { get; set; }
		[GraphQLDescription("Sensors associated with this device.")]
		[Description("Sensors associated with this device.")]
		public ICollection<DeviceSensor>? Sensors { get; set; }
	}
}
