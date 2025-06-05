
using MQTTnet.Server;

namespace Server.Services.Background
{
	public class MqttBackgroundService : BackgroundService
	{
		private readonly MqttService _mqttService;
		public MqttBackgroundService(MqttService mqttService)
		{
			_mqttService = mqttService;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await _mqttService.StartAsync();

			try
			{
				await Task.Delay(Timeout.Infinite, stoppingToken);
			}
			catch (TaskCanceledException)
			{
				Console.WriteLine("Mqtt Shutdown");
			}

			await _mqttService.StopAsync();
		}
	}
}
