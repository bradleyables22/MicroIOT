using Server.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Server.DTOs.SensorCategory
{
	public class CreateSensorCategoryDTO
	{
		[Description("The name of the sensor category.")]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
		[Description("The optional description of the sensor category, if applicable.")]
        [StringLength(500, MinimumLength = 1)]
        public string? Description { get; set; }
		[Description("Metadata associated with this sensor category.")]
		public List<Entry>? Metadata { get; set; }
	}
}
