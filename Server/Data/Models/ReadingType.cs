using Server.Attributes;
using Server.DTOs.ReadingType;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class ReadingType :BaseModel
	{
		public ReadingType()
		{
			
		}

		public ReadingType(CreateReadingTypeDTO create)
		{
			ReadingTypeID = create.ReadingTypeID;
			Name = create.Name;
			Description = create.Description;
			Units = create.Units;
			Metadata = create.Metadata;
			CreatedOn = DateTime.UtcNow;
		}

		public ReadingType(UpdateReadingTypeDTO update)
		{
			ReadingTypeID = update.ReadingTypeID;
			Name = update.Name;
			Description = update.Description;
			Units = update.Units;
			Metadata = update.Metadata;
			CreatedOn = update.CreatedOn;
			DeactivatedOn = update.DeactivatedOn;
		}

		[Key]
		[Description("The reading type ID.")]
		public string ReadingTypeID { get; set; } = Guid.CreateVersion7().ToString("N");
		[Description("The name of the reading type.")]
		[MaxLength(100)]
        [TableColumn("Name", 1)]
        public string Name { get; set; } = string.Empty;
		[Description("The optional description of the reading type.")]
		[MaxLength(500)]
        [TableColumn("Description", 3)]
        public string? Description { get; set; }
		[Description("The unit of measurement, if applicable.")]
		[MaxLength(100)]
        [TableColumn("Units", 2)]
        public string? Units { get; set; }
		[Description("Metadata associated with this reading type.")]
		public List<Entry>? Metadata { get; set; }
		[Description("When the reading type was created.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When the reading type was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }

		[JsonIgnore]
		public ICollection<SensorReading>? Readings { get; set; }

        [NotMapped]
        [JsonIgnore]
        [TableColumn("Status", 4, FalseLabel = "Inactive", TrueLabel = "Active")]
        public bool IsActive
        {
            get
            {
                return DeactivatedOn == null;
            }
        }
    }
}
