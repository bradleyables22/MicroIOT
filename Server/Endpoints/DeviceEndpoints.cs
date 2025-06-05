using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Server.Data.Models;
using Server.DTOs.Device;
using Server.Extensions;
using Server.Models.PushMessages;
using Server.Repositories;
using Server.Services;

namespace Server.Endpoints
{
	public static class DeviceEndpoints
	{
		public static WebApplication MapDeviceEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/Devices").WithTags("Devices");

			group.MapGet("{deviceGroupID}", async (IDeviceRepository _repo, string deviceGroupID) =>
			{
				var result = await _repo.GetWhere(x => x.DeviceGroupID == deviceGroupID);
				return result.AsResponse();
			})
				.Produces<List<Device>>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeviceBydeviceGroup")
				.WithDescription("Get all device belonging to a device group")
				.WithSummary("By Device Group")
				.WithName("DeviceByDeviceGroup")
				;

			group.MapGet("Details/{id}", async (IDeviceRepository _repo, string id) =>
			{
				var result = await _repo.GetById(id);

				return result.AsResponse();
			})
				.Produces<Device>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("GetDeviceByID")
				.WithDescription("Get a specific device by an ID")
				.WithSummary("By ID")
				.WithName("GetDeviceByID")
				;

			group.MapPut("Update", async (IDeviceRepository _repo, UpdateDeviceDTO update) =>
			{
				var result = await _repo.Update(new Device(update));

				return result.AsResponse();
			})
				.Produces<Device>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("UpdateDevice")
				.WithDescription("Update a device")
				.WithSummary("Update")
				.WithName("UpdateDevice")
				;

			group.MapPut("Reactivate", async (IDeviceRepository _repo, string id) =>
			{
				var existingResult = await _repo.GetById(id);
				if (existingResult.Success)
				{
					if (existingResult.Data == null)
						return Results.NotFound();

					if (existingResult.Data.DeactivatedOn == null)
						return Results.Problem(statusCode: 409,
							title: "Conflict",
							detail: "Already deactivated"
							);
					else
					{
						existingResult.Data.DeactivatedOn = null;
						var result = await _repo.Update(existingResult.Data);
						return result.AsResponse();
					}
				}
				else
				{
					return Results.Problem(statusCode: 500,
					title: "Exception",
					detail: existingResult.Exception?.Message);
				}
			})
				.Produces<Device>(200, "application/json")
				.Produces(404)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("ReactivateDevice")
				.WithDescription("Reactivate a device")
				.WithSummary("Reactivate")
				.WithName("ReactivateDevice")
				;

			group.MapPost("Create", async (IDeviceRepository _repo, CreateDeviceDTO create) =>
			{
				var result = await _repo.Create(new Device(create));

				return result.AsResponse();
			})
				.Produces<Device>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateDevice")
				.WithDescription("Create a device")
				.WithSummary("Create")
				.WithName("CreateDevice")
				;
			group.MapPost("MQTT/Push", async (MqttService _mqtt, DevicePushMessage pushItem) =>
			{
				try
				{
					var message = new InjectedMqttApplicationMessage(
					   new MqttApplicationMessage
					   {
						   Topic = $"devicegroup/{pushItem.DeviceGroupID}/device/{pushItem.DeviceID}/commands",
						   Payload = pushItem.GetMessageBytes(),
						   QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
						   Retain = false
					   });


					await _mqtt.PublishAsync(message);

					return Results.Accepted();

				}
				catch (Exception e)
				{
					return Results.Problem(statusCode: 500,
					title: "Exception",
					detail: e.Message);
				}
			})
				.Produces(202)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DevicePush")
				.WithDescription("Push a message to a device. This is published to the topic 'devicegroup/{DeviceGroupID}/device/{DeviceID}/commands'")
				.WithSummary("MQTT Push")
				.WithName("DevicePush")
				;
			group.MapDelete("Deactivate", async (IDeviceRepository _repo, string id) =>
			{
				var existingResult = await _repo.GetById(id);
				if (existingResult.Success)
				{
					if (existingResult.Data == null)
						return Results.NotFound();

					if (existingResult.Data.DeactivatedOn != null)
						return Results.Problem(statusCode: 409,
							title: "Conflict",
							detail: "Already deactivated"
							);
					else
					{
						existingResult.Data.DeactivatedOn = DateTime.UtcNow;
						var result = await _repo.Update(existingResult.Data);
						return result.AsResponse();
					}
				}
				else
				{
					return Results.Problem(statusCode: 500,
					title: "Exception",
					detail: existingResult.Exception?.Message);
				}
			})
				.Produces<Device>(200, "application/json")
				.Produces(404)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeactivateDevice")
				.WithDescription("Deactivate a device")
				.WithSummary("Deactivate")
				.WithName("DeactivateDevice")
				;

			group.MapDelete("Delete/{id}", async (IDeviceRepository _repo, string id) =>
			{
				var result = await _repo.Delete(id);

				return result.AsResponse();
			})
				.Produces<Device>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteDevice")
				.WithDescription("Perminently delete a device")
				.WithSummary("Delete")
				.WithName("DeleteDevice")
				;

			return app;
		}
	}
}
