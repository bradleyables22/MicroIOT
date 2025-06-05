using Server.Data.Models;

namespace Server.Models.Mqtt.Ota
{
	public class OtaMessage
	{
		public OtaMessage()
		{
			
		}

		public OtaMessage(OtaManifestRecord record, string action)
		{
			Version = record.Version;
			url = record.Url;
			DeviceTypeID = record.DeviceTypeID ?? "Unknown";
			Action = action;
		}

		public string Version { get; set; } = string.Empty;
		public string url { get; set; } = string.Empty;
		public string DeviceTypeID { get; set; } = string.Empty;
		public string Action { get; set; } = string.Empty;
	}
}
