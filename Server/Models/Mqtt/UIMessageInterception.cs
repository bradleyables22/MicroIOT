namespace Server.Models.Mqtt
{
    public class UIMessageInterception
    {
        public string DeviceGroupID { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Time { get; set; } = DateTime.UtcNow;
    }
}
