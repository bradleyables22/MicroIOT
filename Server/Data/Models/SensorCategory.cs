using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using Server.Attributes;
using Server.DTOs.SensorCategory;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class SensorCategory:BaseModel
	{
		public SensorCategory()
		{
			
		}

		public SensorCategory(CreateSensorCategoryDTO create)
		{
			SensorCategoryID = 0;
			Name = create.Name;
			Description = create.Description;
			Metadata = create.Metadata;
			CreatedOn = DateTime.UtcNow;
		}

		public SensorCategory(UpdateSensorCategoryDTO update)
		{
			SensorCategoryID = update.SensorCategoryID;
			Name = update.Name;
			Description = update.Description;
			Metadata = update.Metadata;
			CreatedOn = DateTime.UtcNow;
			DeactivatedOn = update.DeactivatedOn;
		}

		[Key]
		[Description("The sensor category ID.")]
		public long SensorCategoryID { get; set; }
		[Description("The name of the sensor category.")]
		[MaxLength(100)]
        [TableColumn("Name", 1)]
        public string Name { get; set; } = string.Empty;
		[Description("The optional description of the sensor category, if applicable.")]
		[MaxLength(500)]
        [TableColumn("Description", 2)]
        public string? Description { get; set; }
		[Description("Metadata associated with this sensor category.")]
		public List<Entry>? Metadata { get; set; }
		[Description("When the sensor category was created.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When the sensor category was deactivated, if applicable.")]
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
