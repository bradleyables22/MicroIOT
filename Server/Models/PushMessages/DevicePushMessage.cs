namespace Server.Models.PushMessages
{
	public class DevicePushMessage :PushMessage
	{
		public string DeviceGroupID { get; set; } = string.Empty;
		public string DeviceID { get; set; } = string.Empty;
	}
}
