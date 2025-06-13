using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Server.DTOs.DeviceSensor;
using Server.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Data.Models
{
	public class DeviceGroupType :BaseModel
	{
		public DeviceGroupType()
		{
			
		}

		public DeviceGroupType(CreateDeviceGroupTypeDTO create)
		{
			GroupTypeID = 0;
			Name = create.Name;
			Description = create.Description;
			Metadata = create.Metadata;
			CreatedOn = DateTime.UtcNow;
		}

		public DeviceGroupType(UpdateDeviceGroupTypeDTO update)
		{
			GroupTypeID = update.GroupTypeID;
			Name = update.Name;
			Description = update.Description;
			Metadata = update.Metadata;
			CreatedOn = update.CreatedOn;
			DeactivatedOn = update.DeactivatedOn;
		}

		[Key]
		[Description("The ID of the device group type.")]
		public long GroupTypeID { get; set; }
		[Description("The name of the device group type.")]
		[MaxLength(100)]
        [TableColumn("Name", 1)]
        public string Name { get; set; } = string.Empty;
		[Description("The optional description of the device group type.")]
		[MaxLength(500)]
        [TableColumn("Description", 2)]
        public string? Description { get; set; }
		[Description("Metadata associated with this group type.")]
		public List<Entry>? Metadata { get; set; }
		[Description("When this group type was created.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When this group type was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[JsonIgnore]
		public ICollection<DeviceGroup>? DeviceGroups { get; set; }

        [NotMapped]
        [JsonIgnore]
        [TableColumn("Status", 3, FalseLabel = "Inactive", TrueLabel = "Active")]
        public bool IsActive
        {
            get
            {
                return DeactivatedOn == null;
            }
        }
    }
}
