using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace Server.Data.Models
{
	public class SystemGroupDocumentation :BaseModel
	{
		[Key]
		[Description("The ID of this document.")]
		public long DocumentID { get; set; }
		[Description("The ID of the system this documentation belongs too.")]
		public long? SystemGroupID { get; set; }
		[Description("The given name of the documentation.")]
		public string Name { get; set; } = string.Empty;
		[Description("The route slug of the documentation.")]
		public string Slug { get; set; } = string.Empty;
		[Description("The optional description of the documentation, if applicable.")]
		[MaxLength(500)]
		public string? Description { get; set; }
		[Description("When the documentation was created on.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When the documentation was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[JsonIgnore]
		[ForeignKey(nameof(SystemGroupID))]
		public SystemGroup? SystemGroup { get; set; } = null!;
		[Description("This document's pages.")]
		public ICollection<DocumentationPage>? Pages { get; set; }
	}
}
