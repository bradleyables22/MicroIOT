namespace Server.Models.Mqtt
{
	public class HandleEndpointResponse
	{
		public bool Success { get; set; } = true;
		public string Reason { get; set; } = string.Empty;
	}
}
