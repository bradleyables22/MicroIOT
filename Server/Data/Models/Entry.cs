
using System.ComponentModel.DataAnnotations;

namespace Server.Data.Models
{
	public class Entry
	{
		[MaxLength(50)]
		[MinLength(1)]
		public string Key { get; set; } = string.Empty;
		[MaxLength(100)]
        [MinLength(1)]
        public string Value { get; set; } = string.Empty;
	}
}
