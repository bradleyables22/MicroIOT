using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Server.Data.Models;
using Server.DTOs.Reading;
using Server.Repositories;
using System.Diagnostics.Contracts;

namespace Server.Services
{
	public class MqttService
	{
		private MqttServer _mqttServer;
		private readonly IServiceScopeFactory _scopeFactory;
		public MqttService(IServiceScopeFactory scopeFactory)
		{

			_scopeFactory = scopeFactory;

			var optionsBuilder = new MqttServerOptionsBuilder()
				.WithDefaultEndpoint()
				.WithDefaultEndpointPort(433)
				.WithoutEncryptedEndpoint()
				;

			_mqttServer = new MqttServerFactory().CreateMqttServer(optionsBuilder.Build());

			_mqttServer.ValidatingConnectionAsync += ValidateConnectionAsync;
			_mqttServer.InterceptingSubscriptionAsync += InterceptSubscriptionAsync;
			_mqttServer.InterceptingPublishAsync += InterceptApplicationMessagePublishAsync;
			_mqttServer.ClientDisconnectedAsync += ClientDisconnectedAsync;
		}

		public async Task PublishAsync(InjectedMqttApplicationMessage message)
		{
			await _mqttServer.InjectApplicationMessage(message);

		}
		public async Task StartAsync() 
		{
			await _mqttServer.StartAsync();
		}

		public async Task StopAsync() 
		{
			await _mqttServer.StopAsync();
		}

		private Task ValidateConnectionAsync(ValidatingConnectionEventArgs args)
		{
			Console.WriteLine($"Client '{args.ClientId}' is connecting...");
			args.ReasonCode = MqttConnectReasonCode.Success;
			return Task.CompletedTask;
		}

		private Task InterceptSubscriptionAsync(InterceptingSubscriptionEventArgs args)
		{
			Console.WriteLine($"Client '{args.ClientId}' is subscribing to '{args.TopicFilter.Topic}'");
			return Task.CompletedTask;
		}

		private async Task InterceptApplicationMessagePublishAsync(InterceptingPublishEventArgs args)
		{
			var topic = args.ApplicationMessage.Topic;
			var payload = args.ApplicationMessage.ConvertPayloadToString();

			Console.WriteLine($"Message published to '{topic}': {payload}");

			if (topic == "readings")
			{
				using var scope = _scopeFactory.CreateScope();
				var repo = scope.ServiceProvider.GetRequiredService<ISensorReadingRepository>();
				var reason = string.Empty;
				try
				{
					var reading = System.Text.Json.JsonSerializer.Deserialize<CreateSensorReadingDTO>(payload);
					if (reading != null)
					{
						var result = await repo.Create(new SensorReading(reading));
						if (result.Success && result.Data != null)
						{
							reason = "Success";
							args.Response.ReasonCode = MqttPubAckReasonCode.Success;
							Console.WriteLine($"Sensor {result.Data.SensorID} successfully published data");
						}
						else
						{
							reason = $"Sensor {result?.Data?.SensorID} failed to publish {payload}";
							args.Response.ReasonCode = MqttPubAckReasonCode.UnspecifiedError;
							Console.WriteLine($"Sensor {result?.Data?.SensorID} failed to publish {payload}");
						}
					}
					else
					{
						reason = $"Unknown sensor failed to publish {payload}";
						args.Response.ReasonCode = MqttPubAckReasonCode.UnspecifiedError;
						args.Response.ReasonString = reason;
						Console.WriteLine(reason);
					}
				}
				catch (Exception ex)
				{
					reason = $"Sensor publishing failure: {ex.Message}";
					args.Response.ReasonCode = MqttPubAckReasonCode.UnspecifiedError;
					args.Response.ReasonString = reason;
					Console.WriteLine(reason);
				}
			}
		}

		private Task ClientDisconnectedAsync(ClientDisconnectedEventArgs args)
		{
			Console.WriteLine($"Client '{args.ClientId}' disconnected.");
			return Task.CompletedTask;
		}


	}
}
