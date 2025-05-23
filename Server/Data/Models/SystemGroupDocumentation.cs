using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Data.Models
{
	public class SystemGroupDocumentation :BaseModel
	{
		[Key]
		public long DocumentID { get; set; }
		public long? SystemGroupID { get; set; }
		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public DateTime CreatedOn { get; set; }

		public DateTime? DeactivatedOn { get; set; }
		[JsonIgnore]
		[ForeignKey(nameof(SystemGroupID))]
		public SystemGroup? SystemGroup { get; set; } = null!;
		public ICollection<DocumentationPage>? Pages { get; set; }
	}
}
