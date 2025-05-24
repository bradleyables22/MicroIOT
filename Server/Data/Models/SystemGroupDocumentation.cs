using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace Server.Data.Models
{
	public class SystemGroupDocumentation :BaseModel
	{
		[Key]
		[GraphQLDescription("The ID of this document.")]
		[Description("The ID of this document.")]
		public long DocumentID { get; set; }
		[GraphQLDescription("The ID of the system this documentation belongs too.")]
		[Description("The ID of the system this documentation belongs too.")]
		public long? SystemGroupID { get; set; }
		[GraphQLDescription("The given name of the documentation.")]
		[Description("The given name of the documentation.")]
		public string Name { get; set; } = string.Empty;
		[GraphQLDescription("The route slug of the documentation.")]
		[Description("The route slug of the documentation.")]
		public string Slug { get; set; } = string.Empty;
		[GraphQLDescription("A short description of the documentation's purpose.")]
		[Description("The optional description of the documentation, if applicable.")]
		[MaxLength(500)]
		public string? Description { get; set; }
		[GraphQLDescription("When the documentation was created on.")]
		[Description("When the documentation was created on.")]
		public DateTime CreatedOn { get; set; }
		[GraphQLDescription("When the documentation was deactivated, if applicable.")]
		[Description("When the documentation was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }
		[JsonIgnore]
		[GraphQLIgnore]
		[ForeignKey(nameof(SystemGroupID))]
		public SystemGroup? SystemGroup { get; set; } = null!;
		[GraphQLDescription("This document's pages.")]
		[Description("This document's pages.")]
		public ICollection<DocumentationPage>? Pages { get; set; }
	}
}
