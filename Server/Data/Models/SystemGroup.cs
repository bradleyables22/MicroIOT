using Server.DTOs.SystemGroup;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Server.Data.Models
{
	public class SystemGroup :BaseModel
	{
		public SystemGroup()
		{
				
		}

		public SystemGroup(CreateSystemGroupDTO create)
		{
			SystemGroupID = 0;
			Name = create.Name;
			Description = create.Description;
			Metadata = create.Metadata;
			CreatedOn = DateTime.UtcNow;
		}

		public SystemGroup(UpdateSystemGroupDTO update)
		{
			SystemGroupID = update.SystemGroupID;
			Name = update.Name;
			Description = update.Description;
			Metadata = update.Metadata;
			DeactivatedOn = DateTime.UtcNow;
			CreatedOn = update.CreatedOn;
		}

		[Key]
		[Description("The ID of the system group.")]
		public long SystemGroupID { get; set; }
		[Description("The name of the system group.")]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		[Description("A short description of the systemgroup.")]
		[MaxLength(500)]
		public string Description { get; set; } = string.Empty;
		[Description("User defined metadata related to this system group.")]
		public List<Entry>? Metadata { get; set; }
		[Description("When the system group was created on.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When the system group was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[Description("Device groups belonging to this system group.")]
		public ICollection<DeviceGroup>? DeviceGroups { get; set; }
	}
}
