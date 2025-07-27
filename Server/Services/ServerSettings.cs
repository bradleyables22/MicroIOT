namespace Server.Services
{
    public class ServerSettings
    {
        public bool HideDeactivatedItems { get; set; } = false;
        public bool UseMqttAuthentication {get;set;} = false;
        public bool UseHttpsAuthentication { get; set; } = false;

    }
}
