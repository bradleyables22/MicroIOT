using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class ReadingType :BaseModel
	{
		[Key]
		[GraphQLDescription("The reading type ID.")]
		[Description("The reading type ID.")]
		public string ReadingTypeID { get; set; } = Guid.CreateVersion7().ToString("N");
		[GraphQLDescription("The name of the reading type.")]
		[Description("The name of the reading type.")]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		[GraphQLDescription("The optional description of the reading type.")]
		[Description("The optional description of the reading type.")]
		[MaxLength(500)]
		public string? Description { get; set; }
		[GraphQLDescription("The unit of measurement, if applicable.")]
		[Description("The unit of measurement, if applicable.")]
		[MaxLength(100)]
		public string? Units { get; set; }
		[GraphQLDescription("Metadata associated with this reading type.")]
		[Description("Metadata associated with this reading type.")]
		public List<Entry>? Metadata { get; set; }
		[GraphQLDescription("When the reading type was created.")]
		[Description("When the reading type was created.")]
		public DateTime CreatedOn { get; set; }
		[GraphQLDescription("When the reading type was deactivated, if applicable.")]
		[Description("When the reading type was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }

		[GraphQLIgnore]
		[JsonIgnore]
		public ICollection<SensorReading>? Readings { get; set; }
	}
}
