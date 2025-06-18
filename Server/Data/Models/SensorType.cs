using Server.Attributes;
using Server.DTOs.SensorType;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class SensorType:BaseModel
	{
		public SensorType()
		{
				
		}

		public SensorType(CreateSensorTypeDTO create)
		{
			SensorTypeID = create.SensorTypeID;
			Name = create.Name;
			Description = create.Description;
			Metadata = create.Metadata;
			CreatedOn = DateTime.UtcNow;
		}

		public SensorType(UpdateSensorTypeDTO update)
		{
			SensorTypeID = update.SensorTypeID;
			Name = update.Name;
			Description = update.Description;
			Metadata = update.Metadata;
			CreatedOn = update.CreatedOn;
			DeactivatedOn = DateTime.UtcNow;
		}

		[Key]
		[Description("The ID of the sensor type.")]
		public string SensorTypeID { get; set; } = Guid.CreateVersion7().ToString();
		[Description("The the name of the sensor type.")]
		[MaxLength(100)]
        [TableColumn("Name", 1)]
        public string Name { get; set; } = string.Empty;
		[Description("The optional description of the sensor type, if applicable.")]
		[MaxLength(500)]
        [TableColumn("Description", 2)]
        public string? Description { get; set; }
		[Description("Metadata associated with these sensor types.")]
		public List<Entry>? Metadata { get; set; }
		[Description("When this sensor type was created.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When this sensor type was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[JsonIgnore]
		public ICollection<DeviceSensor>? DeviceSensors { get; set; }

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
