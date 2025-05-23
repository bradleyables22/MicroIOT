using Microsoft.EntityFrameworkCore;

namespace Server.Data.Models
{
	public class Entry
	{
		public string Key { get; set; } = string.Empty;
		public string Value { get; set; } = string.Empty;
	}
}
