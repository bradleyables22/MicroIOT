using Microsoft.AspNetCore.Mvc;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Server.Data.Models;
using Server.DTOs.DeviceType;
using Server.Extensions;
using Server.Models.PushMessages;
using Server.Repositories;
using Server.Services;

namespace Server.Endpoints
{
	public static class DeviceTypeEndpoints
	{
		public static WebApplication MapDeviceTypeEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/DeviceTypes").WithTags("Device Types");

			group.MapGet("", async (IDeviceTypeRepository _repo, [FromQuery] bool? activeOnly) =>
			{
				var result = !Convert.ToBoolean(activeOnly) ? await _repo.GetAll() : await _repo.GetWhere(x => x.DeactivatedOn == null);
				return result.AsResponse();
			})
				.Produces<List<DeviceType>>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeviceTypes")
				.WithDescription("Get all device types")
				.WithSummary("All")
				.WithName("DeviceTypes")
				;

			group.MapGet("Details/{id}", async (IDeviceTypeRepository _repo, string id) =>
			{
				var result = await _repo.GetById(id);

				return result.AsResponse();
			})
				.Produces<DeviceType>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("GetDeviceTypeByID")
				.WithDescription("Get a specific device type by an ID")
				.WithSummary("By ID")
				.WithName("GetDeviceTypeByID")
				;

			group.MapPut("Update", async (IDeviceTypeRepository _repo, UpdateDeviceTypeDTO update) =>
			{
				var result = await _repo.Update(new DeviceType(update));

				return result.AsResponse();
			})
				.Produces<DeviceType>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("UpdateDeviceType")
				.WithDescription("Update a device type")
				.WithSummary("Update")
				.WithName("UpdateDeviceType")
				;

			group.MapPut("Reactivate", async (IDeviceTypeRepository _repo, string id) =>
			{
				var existingResult = await _repo.GetById(id);
				if (existingResult.Success)
				{
					if (existingResult.Data == null)
						return Results.NotFound();

					if (existingResult.Data.DeactivatedOn == null)
						return Results.Problem(statusCode: 409,
							title: "Conflict",
							detail: "Currently Active"
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
				.Produces<DeviceType>(200, "application/json")
				.Produces(404)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("ReactivateDeviceType")
				.WithDescription("Reactivate a device type")
				.WithSummary("Reactivate")
				.WithName("ReactivateDeviceType")
				;

			group.MapPost("Create", async (IDeviceTypeRepository _repo, CreateDeviceTypeDTO create) =>
			{
				var result = await _repo.Create(new DeviceType(create));

				return result.AsResponse();
			})
				.Produces<DeviceType>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateDeviceType")
				.WithDescription("Create a device type")
				.WithSummary("Create")
				.WithName("CreateDeviceType")
				;

			group.MapPost("MQTT/Push", async (MqttService _mqtt, DeviceTypePushMessage pushItem) =>
			{
				try
				{
					var message = new InjectedMqttApplicationMessage(
					   new MqttApplicationMessage
					   {
						   Topic = $"devicetype/{pushItem.DeviceTypeID}/commands",
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
				.WithDisplayName("DeviceTypePush")
				.WithDescription("Push a message to a device type. This is published to the topic 'devicetype/{DeviceTypeID}/commands'")
				.WithSummary("MQTT Push")
				.WithName("DeviceTypePush")
				;

			group.MapDelete("Deactivate", async (IDeviceTypeRepository _repo, string id) =>
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
				.Produces<DeviceType>(200, "application/json")
				.Produces(404)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeactivateDeviceType")
				.WithDescription("Deactivate a device type")
				.WithSummary("Deactivate")
				.WithName("DeactivateDeviceType")
				;

			group.MapDelete("Delete", async (IDeviceTypeRepository _repo, string id) =>
			{
				var result = await _repo.Delete(id);

				return result.AsResponse();
			})
				.Produces<DeviceType>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteDeviceType")
				.WithDescription("Perminently delete a device type")
				.WithSummary("Delete")
				.WithName("DeleteDeviceType")
				;

			return app;
		}
	}
}
