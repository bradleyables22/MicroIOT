using Server.DTOs.DeviceType;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class DeviceType:BaseModel
	{
		public DeviceType()
		{
			
		}
		public DeviceType(CreateDeviceTypeDTO create)
		{
			DeviceTypeID = create.DeviceTypeID;
			Name = create.Name;
			Description = create.Description;
			Metadata = create.Metadata;
			CreatedOn = DateTime.UtcNow;
		}

		public DeviceType(UpdateDeviceTypeDTO update)
		{
			DeviceTypeID = update.DeviceTypeID;
			Name = update.Name;
			Description = update.Description;
			Metadata = update.Metadata;
			CreatedOn = update.CreatedOn;
			DeactivatedOn = update.DeactivatedOn;
		}

		[Key]
		[Description("The ID of the device type.")]
		public string DeviceTypeID { get; set; } = Guid.CreateVersion7().ToString();
		[Description("The name of the device type.")]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		[Description("The optional description of the device type.")]
		[MaxLength(500)]
		public string? Description { get; set; }
		[Description("Metadata associated with the device type.")]
		public List<Entry>? Metadata { get; set; }
		[Description("When the device type was created.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When the device type was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[JsonIgnore]
		public ICollection<Device>? Devices { get; set; }
		[JsonIgnore]
		public ICollection<OtaManifestRecord>? Ota_Manifests { get; set; }
	}
}
