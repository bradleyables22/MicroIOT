using Server.Data.Models;

namespace Server.Components.Models
{
    public class EditableDeviceGroupType
    {
        public EditableDeviceGroupType()
        {

        }
        public EditableDeviceGroupType(DeviceGroupType dg)
        {
            GroupTypeID = dg.GroupTypeID;
            Name = dg.Name;
            Description = dg.Description;
            CreatedOn = dg.CreatedOn;
            DeactivatedOn = dg.DeactivatedOn;
            Metadata = dg.Metadata;
        }

        public long GroupTypeID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<Entry>? Metadata { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? DeactivatedOn { get; set; }

        public DeviceGroupType GetDatabaseModel() 
        {
            return new DeviceGroupType
            {
               GroupTypeID = this.GroupTypeID,
               Name = this.Name,
               Description = this.Description,
               CreatedOn = this.CreatedOn,
               DeactivatedOn = this.DeactivatedOn,
               Metadata = this.Metadata
            };
        }
    }
}
