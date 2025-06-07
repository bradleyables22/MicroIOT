using Microsoft.AspNetCore.Http.HttpResults;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using MQTTnet.Server.Internal;
using Server.Data.DTOs.OtaDownload;
using Server.Data.Models;
using Server.DTOs.Device;
using Server.DTOs.DeviceGroup;
using Server.DTOs.DeviceSensor;
using Server.DTOs.Reading;
using Server.Extensions;
using Server.Models.Mqtt;
using Server.Repositories;
using System.Text.Json;

namespace Server.Services
{
	public class MqttService
	{
		public MqttServer _mqttServer;
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly List<string> crudWords = new List<string>() { "create", "update", "delete"};
		public MqttService(IServiceScopeFactory scopeFactory)
		{

			_scopeFactory = scopeFactory;

			var optionsBuilder = new MqttServerOptionsBuilder()
				.WithDefaultEndpoint()
				.WithDefaultEndpointPort(433)
				.WithDefaultEndpointPort(80)
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
			if (!_mqttServer.IsStarted)
				await _mqttServer.StartAsync();
		}

		public async Task StopAsync() 
		{
			if (_mqttServer.IsStarted)
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
			
			try
			{
				var topic = args.ApplicationMessage.Topic;
				var payload = args.ApplicationMessage.ConvertPayloadToString();

				HandleEndpointResponse response = new();

				switch (topic.Split("/").FirstOrDefault())
				{
					case "devicegroups":
						response = await HandleDeviceGroupPublish(topic, payload, args);
						break;
					case "devices":
						response = await HandleDevicePublish(topic, payload, args);
						break;
					case "sensors":
						response = await HandleDeviceSensorPublish(topic, payload, args);
						break;
					case "readings":
						response = await HandleReadingsPublish(topic, payload, args);
						break;
					case "downloads":
						response = await HandleOtaPublish(topic, payload, args);
						break;
					default:
						break;
				}

				args.Response.ReasonCode = response.Success ? MqttPubAckReasonCode.Success : MqttPubAckReasonCode.UnspecifiedError;
				args.Response.ReasonString = response.Reason;
			}
			catch (Exception)
			{
				args.Response.ReasonCode = MqttPubAckReasonCode.UnspecifiedError;
				args.Response.ReasonString = "Exception Thrown";
			}
			finally 
			{
				Console.WriteLine($"{args.ClientId} published to {args.ApplicationMessage.Topic}");
			} 
		}

		private Task ClientDisconnectedAsync(ClientDisconnectedEventArgs args)
		{
			Console.WriteLine($"Client '{args.ClientId}' disconnected.");
			return Task.CompletedTask;
		}

		private async Task<HandleEndpointResponse> HandleDeviceGroupPublish(string topic, string? payload, InterceptingPublishEventArgs args) 
		{
			bool succuss = true;
			string reason = string.Empty;
			try
			{
				var endpoint = topic?.Split("/").Skip(1).FirstOrDefault();
				using var scope = _scopeFactory.CreateScope();
				var repo = scope.ServiceProvider.GetRequiredService<IDeviceGroupRepository>();
				string detailsString = $"details/{args.ClientId}";


				if (topic.Contains(detailsString))
				{
					var result = await repo.GetById(args.ClientId);
					if (result.Success && result.Data != null)
					{
						var message = new InjectedMqttApplicationMessage(
						   new MqttApplicationMessage
						   {
							   Topic = $"get/devicegroup/{args.ClientId}",
							   Payload = JsonSerializer.Serialize(result.Data).GetByteSequence(),
							   QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
							   Retain = false
						   });

						await PublishAsync(message);
						reason = $"get/devicegroup/{args.ClientId}";
					}
					else
					{
						succuss = false;
						reason = "Get Failure";
					}
				}
				else
				{
					switch (endpoint)
					{
						case "create":
							var createObj = JsonSerializer.Deserialize<CreateDeviceGroupDTO>(payload);
							var createResult = await repo.Create(new DeviceGroup(createObj));
							if (!createResult.Success)
							{
								succuss = false;
								reason = "Create Failure";
							}
							break;
						case "update":
							var updateObj = JsonSerializer.Deserialize<UpdateDeviceGroupDTO>(payload);
							var updateResult = await repo.Update(new DeviceGroup(updateObj));
							if (!updateResult.Success)
							{
								succuss = false;
								reason = "Update Failure";
							}
							break;
						case "delete":
							var deleteResult = await repo.Delete(args.ClientId);
							if (!deleteResult.Success)
							{
								succuss = false;
								reason = "Delete Failure";
							}
							break;
						default:
							break;
					}
				}
				return new HandleEndpointResponse { Success = succuss, Reason = reason };
			}
			catch (Exception)
			{
				return new HandleEndpointResponse { Success = false, Reason = "exception thrown" };
			}
		}

		private async Task<HandleEndpointResponse> HandleDevicePublish(string topic, string? payload, InterceptingPublishEventArgs args)
		{
			bool succuss = true;
			string reason = string.Empty;
			try
			{
				var endpoint = topic?.Split("/").Skip(1).FirstOrDefault();
				using var scope = _scopeFactory.CreateScope();
				var repo = scope.ServiceProvider.GetRequiredService<IDeviceRepository>();
				string detailsString = $"details";


				if (topic.Contains(detailsString))
				{
					var id = topic?.Split("/").Skip(2).FirstOrDefault();

					var result = await repo.GetById(id);
					if (result.Success && result.Data != null)
					{
						var message = new InjectedMqttApplicationMessage(
						   new MqttApplicationMessage
						   {
							   Topic = $"get/devicegroup/{args.ClientId}/device/{id}",
							   Payload = JsonSerializer.Serialize(result.Data).GetByteSequence(),
							   QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
							   Retain = false
						   });

						await PublishAsync(message);
						reason = $"get/devicegroup/{args.ClientId}/device/{id}";
					}
					else
					{
						succuss = false;
						reason = "Get Failure";
					}
				}
				else if (endpoint == args.ClientId) 
				{
					var result = await repo.GetWhere(x => x.DeviceGroupID == args.ClientId);

					if (result.Success && result.Data != null)
					{
						var message = new InjectedMqttApplicationMessage(
						   new MqttApplicationMessage
						   {
							   Topic = $"get/devicegroup/{args.ClientId}/devices",
							   Payload = JsonSerializer.Serialize(result.Data).GetByteSequence(),
							   QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
							   Retain = false
						   });

						await PublishAsync(message);
						reason = $"get/devicegroup/{args.ClientId}/devices";
					}
					else
					{
						succuss = false;
						reason = "Get Failure";
					}
				}
				else
				{
					switch (endpoint)
					{
						case "create":
							var createObj = JsonSerializer.Deserialize<CreateDeviceDTO>(payload);
							var createResult = await repo.Create(new Device(createObj));
							if (!createResult.Success)
							{
								succuss = false;
								reason = "Create Failure";
							}
							break;
						case "update":
							var updateObj = JsonSerializer.Deserialize<UpdateDeviceDTO>(payload);
							var updateResult = await repo.Update(new Device(updateObj));
							if (!updateResult.Success)
							{
								succuss = false;
								reason = "Update Failure";
							}
							break;
						case "delete":
							var deleteResult = await repo.Delete(topic.Split("/").Skip(2).FirstOrDefault());
							if (!deleteResult.Success)
							{
								succuss = false;
								reason = "Delete Failure";
							}
							break;
						default:
							break;
					}
				}
				return new HandleEndpointResponse { Success = succuss, Reason = reason };
			}
			catch (Exception)
			{
				return new HandleEndpointResponse { Success = false, Reason = "exception thrown" };
			}
		}

		private async Task<HandleEndpointResponse> HandleDeviceSensorPublish(string topic, string? payload, InterceptingPublishEventArgs args)
		{
			bool succuss = true;
			string reason = string.Empty;
			try
			{
				var endpoint = topic?.Split("/").Skip(1).FirstOrDefault();
				using var scope = _scopeFactory.CreateScope();
				var repo = scope.ServiceProvider.GetRequiredService<IDeviceSensorRepository>();
				string detailsString = $"details";

				if (topic.Contains(detailsString))
				{
					var id = topic?.Split("/").Skip(2).FirstOrDefault();

					var result = await repo.GetById(id);
					if (result.Success && result.Data != null)
					{
						var message = new InjectedMqttApplicationMessage(
						   new MqttApplicationMessage
						   {
							   Topic = $"get/devicegroup/{args.ClientId}/device/{endpoint}/sesnor/{id}",
							   Payload = JsonSerializer.Serialize(result.Data).GetByteSequence(),
							   QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
							   Retain = false
						   });

						await PublishAsync(message);
						reason = $"get/devicegroup/{args.ClientId}/device/{endpoint}/sensor/{id}";
					}
					else
					{
						succuss = false;
						reason = "Get Failure";
					}
				}
				else if (crudWords.Contains(endpoint))
				{
					switch (endpoint)
					{
						case "create":
							var createObj = JsonSerializer.Deserialize<CreateDeviceSensorDTO>(payload);
							var createResult = await repo.Create(new DeviceSensor(createObj));
							if (!createResult.Success)
							{
								succuss = false;
								reason = "Create Failure";
							}
							break;
						case "update":
							var updateObj = JsonSerializer.Deserialize<UpdateDeviceSensorDTO>(payload);
							var updateResult = await repo.Update(new DeviceSensor(updateObj));
							if (!updateResult.Success)
							{
								succuss = false;
								reason = "Update Failure";
							}
							break;
						case "delete":
							var deleteResult = await repo.Delete(topic.Split("/").Skip(2).FirstOrDefault());
							if (!deleteResult.Success)
							{
								succuss = false;
								reason = "Delete Failure";
							}
							break;
						default:
							break;
					}
				}
				else if (!string.IsNullOrEmpty(endpoint))
				{
					var result = await repo.GetWhere(x => x.DeviceID == endpoint);

					if (result.Success && result.Data != null)
					{
						var message = new InjectedMqttApplicationMessage(
						   new MqttApplicationMessage
						   {
							   Topic = $"get/devicegroup/{args.ClientId}/device/{endpoint}/sensors",
							   Payload = JsonSerializer.Serialize(result.Data).GetByteSequence(),
							   QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
							   Retain = false
						   });

						await PublishAsync(message);
						reason = $"get/devicegroup/{args.ClientId}/device/{endpoint}/sensors";
					}
					else
					{
						succuss = false;
						reason = "Get Failure";
					}
				}


				return new HandleEndpointResponse { Success = succuss, Reason = reason };
			}
			catch (Exception)
			{
				return new HandleEndpointResponse { Success = false, Reason = "exception thrown" };
			}
		}

		private async Task<HandleEndpointResponse> HandleOtaPublish(string topic, string? payload, InterceptingPublishEventArgs args) 
		{
			bool succuss = true;
			string reason = string.Empty;
			try
			{
				var endpoint = topic?.Split("/").Skip(1).FirstOrDefault();
				using var scope = _scopeFactory.CreateScope();
				var repo = scope.ServiceProvider.GetRequiredService<IOtaDownloadRepository>();

				switch (endpoint)
				{
					case "create":
						var createObj = JsonSerializer.Deserialize<CreateOtaDownloadRecordDTO>(payload);
						var createResult = await repo.Create(new OtaDownloadRecord(createObj));
						if (!createResult.Success)
						{
							succuss = false;
							reason = "Create Failure";
						}
						break;
					case "update":
						var updateObj = JsonSerializer.Deserialize<UpdateOtaDownloadRecordDTO>(payload);
						var updateResult = await repo.Update(new OtaDownloadRecord(updateObj));
						if (!updateResult.Success)
						{
							succuss = false;
							reason = "Update Failure";
						}
						break;
					case "delete":
						var deleteResult = await repo.Delete(Convert.ToInt64(topic.Split("/").Skip(2).FirstOrDefault()));
						if (!deleteResult.Success)
						{
							succuss = false;
							reason = "Delete Failure";
						}
						break;
					default:
						break;
				}

				return new HandleEndpointResponse { Success = succuss, Reason = reason };
			}
			catch (Exception)
			{
				return new HandleEndpointResponse { Success = false, Reason = "Exception Thrown" };
			}
		}

		private async Task<HandleEndpointResponse> HandleReadingsPublish(string topic, string? payload, InterceptingPublishEventArgs args)
		{
			bool succuss = true;
			string reason = string.Empty;
			try
			{
				var endpoint = topic?.Split("/").Skip(1).FirstOrDefault();
				using var scope = _scopeFactory.CreateScope();
				var repo = scope.ServiceProvider.GetRequiredService<ISensorReadingRepository>();

				switch (endpoint)
				{
					case "create":
						var createObj = JsonSerializer.Deserialize<CreateSensorReadingDTO>(payload);
						var createResult = await repo.Create(new SensorReading(createObj));
						if (!createResult.Success)
						{
							succuss = false;
							reason = "Create Failure";
						}
						break;
					case "update":
						var updateObj = JsonSerializer.Deserialize<UpdateSensorReadingDTO>(payload);

						var existingResult = await repo.GetById(updateObj.ReadingID);
						if (existingResult.Success && existingResult.Data != null)
						{
							existingResult.Data.SensorID = updateObj.SensorID;
							existingResult.Data.Value = updateObj.Value;
							existingResult.Data.Metadata = updateObj.Metadata;
							existingResult.Data.ReadingTypeID = updateObj.ReadingTypeID;

							var updateResult = await repo.Update(existingResult.Data);
						}
						else
						{
							succuss = false;
							reason = "Update Failure";
						}
						break;
					default:
						break;
				}

				return new HandleEndpointResponse { Success = succuss, Reason = reason };
			}
			catch (Exception)
			{
				return new HandleEndpointResponse { Success = false, Reason = "exception thrown" };
			}
		}
	}
}
