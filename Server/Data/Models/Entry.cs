
using System.ComponentModel.DataAnnotations;

namespace Server.Data.Models
{
	public class Entry
	{
		[MaxLength(100)]
		public string Key { get; set; } = string.Empty;
		[MaxLength(500)]
		public string Value { get; set; } = string.Empty;
	}
}
