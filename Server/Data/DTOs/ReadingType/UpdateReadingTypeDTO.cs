using Server.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Server.DTOs.ReadingType
{
	public class UpdateReadingTypeDTO
	{
		[Description("The reading type ID.")]
		public string ReadingTypeID { get; set; } = Guid.CreateVersion7().ToString("N");
		[Description("The name of the reading type.")]
		[StringLength(100, MinimumLength = 1)]
		public string Name { get; set; } = string.Empty;
		[Description("The optional description of the reading type.")]
		[StringLength(500, MinimumLength = 1)]
		public string? Description { get; set; }
		[Description("The unit of measurement, if applicable.")]
		[StringLength(100, MinimumLength = 1)]
		public string? Units { get; set; }
		[Description("Metadata associated with this reading type.")]
		public List<Entry>? Metadata { get; set; }
		[Description("When the reading type was created.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When the reading type was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
	}
}
