using Server.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Server.DTOs.SensorType
{
	public class CreateSensorTypeDTO
	{
		[Description("The ID of the sensor type.")]
		public string SensorTypeID { get; set; } = Guid.CreateVersion7().ToString();
		[Description("The the name of the sensor type.")]
		[StringLength(100, MinimumLength = 1)]
		public string Name { get; set; } = string.Empty;
		[Description("The optional description of the sensor type, if applicable.")]
		[StringLength(500, MinimumLength = 1)]
		public string? Description { get; set; }
		[Description("Metadata associated with these sensor types.")]
		public List<Entry>? Metadata { get; set; }
	}
}
