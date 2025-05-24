using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace Server.Data.Models
{
	public class DocumentationPage:BaseModel
	{
		[Key]
		[GraphQLDescription("The page's ID.")]
		[Description("The page's ID.")]
		public long PageID { get; set; }
		[GraphQLDescription("The parent document's ID.")]
		[Description("The parent document's ID.")]
		public long? DocumentID { get; set; }
		[Description("The title of the page.")]
		[GraphQLDescription("The title of the page.")]
		[MaxLength(100)]
		public string Title { get; set; } = string.Empty;
		[GraphQLDescription("The route slug of the page.")]
		[Description("The route slug of the page.")]
		[MaxLength(100)]
		public string Slug { get; set; } = string.Empty;
		[GraphQLDescription("The pages content.")]
		[Description("The pages content.")]
		public string Content { get; set; } = string.Empty;
		[GraphQLDescription("When the documentation last updated.")]
		[Description("When the documentation last updated.")]
		public DateTime? UpdatedOn { get; set; }
		[GraphQLDescription("When this page was created.")]
		[Description("When this page was created.")]
		public DateTime CreatedOn { get; set; }
		[GraphQLDescription("When this page was deactivated, if applicable.")]
		[Description("When this page was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }

		[GraphQLIgnore]
		[JsonIgnore]
		[ForeignKey(nameof(DocumentID))]
		public SystemGroupDocumentation? Document { get; set; }
	}
}
