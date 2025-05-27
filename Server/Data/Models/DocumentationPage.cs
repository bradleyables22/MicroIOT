using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace Server.Data.Models
{
	public class DocumentationPage:BaseModel
	{
		[Key]
		[Description("The page's ID.")]
		public long PageID { get; set; }
		[Description("The parent document's ID.")]
		public long? DocumentID { get; set; }
		[Description("The title of the page.")]
		[MaxLength(100)]
		public string Title { get; set; } = string.Empty;
		[Description("The route slug of the page.")]
		[MaxLength(100)]
		public string Slug { get; set; } = string.Empty;
		[Description("The pages content.")]
		public string Content { get; set; } = string.Empty;
		[Description("When the documentation last updated.")]
		public DateTime? UpdatedOn { get; set; }
		[Description("When this page was created.")]
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		[Description("When this page was deactivated, if applicable.")]
		public DateTime? DeactivatedOn { get; set; }

		[JsonIgnore]
		[ForeignKey(nameof(DocumentID))]
		public SystemGroupDocumentation? Document { get; set; }
	}
}
