namespace Server.Models.PushMessages
{
	public class DeviceSensorPushMessage : PushMessage
	{
		public string DeviceGroupID { get; set; } = string.Empty;
		public string DeviceID { get; set; } = string.Empty;
		public string SensorID { get; set; } = string.Empty;
	}
}
