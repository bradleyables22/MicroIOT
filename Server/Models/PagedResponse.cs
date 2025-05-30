namespace Server.Models
{
	public class PagedResponse<T>
	{
		public int Count { get; set; }
		public long? Last { get; set; }
		public List<T>? Data { get; set; }
		public bool End { get; set; }
	}
}
