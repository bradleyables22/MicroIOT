using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel;
using Server.DTOs.DeviceGroup;

namespace Server.Data.Models
{
	public class DeviceGroup:BaseModel
	{
		public DeviceGroup()
		{
			
		}
		public DeviceGroup(CreateDeviceGroupDTO create)
		{
			DeviceGroupID = create.DeviceGroupID;
			Alias = create.Alias;
			SystemGroupID = create.SystemGroupID;
			GroupTypeID = create.DeviceGroupTypeID;
			Metadata = create.Metadata;
			CreatedOn = DateTime.UtcNow;
		}
		public DeviceGroup(UpdateDeviceGroupDTO create)
		{
			DeviceGroupID = create.DeviceGroupID;
			Alias = create.Alias;
			SystemGroupID = create.SystemGroupID;
			GroupTypeID = create.DeviceGroupTypeID;
			Metadata = create.Metadata;
			CreatedOn = DateTime.UtcNow;
		}


		[Key]
		[Description("The device group ID.")]
		public string DeviceGroupID { get; set; } = Guid.CreateVersion7().ToString("N");
		[Description("The given alias of this device group.")]
		[MaxLength(100)]
		public string Alias { get; set; } = string.Empty;
		[Description("The system group this device belongs to.")]
		public long? SystemGroupID { get; set; }
		[Description("The device group type this device belongs to.")]
		public long? GroupTypeID { get; set; }
		[Description("The device group type this device belongs to.")]
		public List<Entry>? Metadata { get; set; }
		[Description("The device group type this device belongs to.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When this device group confirmed it has received all system identifiers.")]
		public DateTime? ConfirmedOn { get; set; }
		[Description("When this device group was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[Description("The device group type this device group belongs too.")]
		[ForeignKey(nameof(GroupTypeID))]
		public DeviceGroupType? GroupType { get; set; }
		[JsonIgnore]
		[ForeignKey(nameof(SystemGroupID))]
		public SystemGroup? SystemGroup { get; set; }
		[Description("Child devices of this device group.")]
		public ICollection<Device>? Devices { get; set; }
	}
}
