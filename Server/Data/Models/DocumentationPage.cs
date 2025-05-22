﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Server.Data.Models
{
	public class DocumentationPage:BaseModel
	{
		[Key]
		public long PageID { get; set; }

		public long DocumentID { get; set; }

		public string Title { get; set; } = string.Empty;

		public string Slug { get; set; } = string.Empty;

		public string Content { get; set; } = string.Empty;

		public DateTime CreatedOn { get; set; }

		public DateTime? DeactivatedOn { get; set; }

		[ForeignKey(nameof(DocumentID))]
		public SystemGroupDocumentation Document { get; set; } = null!;
	}
}
