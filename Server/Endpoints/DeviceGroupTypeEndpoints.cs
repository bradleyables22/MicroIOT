using Microsoft.AspNetCore.Mvc;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Server.Data.Models;
using Server.DTOs.DeviceSensor;
using Server.Extensions;
using Server.Models.PushMessages;
using Server.Repositories;
using Server.Services;

namespace Server.Endpoints
{
	public static class DeviceGroupTypeEndpoints
	{
		public static WebApplication MapDeviceGroupTypeEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/DeviceGroupTypes").WithTags("Device Group Types");

			group.MapGet("", async (IDeviceGroupTypeRepository _repo, [FromQuery] bool? activeOnly) =>
			{
				var result = !Convert.ToBoolean(activeOnly) ? await _repo.GetAll() : await _repo.GetWhere(x => x.DeactivatedOn == null);
				return result.AsResponse();
			})
				.Produces<List<DeviceGroupType>>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeviceGroupTypes")
				.WithDescription("Get all device group types")
				.WithSummary("All")
				.WithName("DeviceGroupTypes")
				;

			group.MapGet("Details/{id}", async (IDeviceGroupTypeRepository _repo, long id) =>
			{
				var result = await _repo.GetById(id);

				return result.AsResponse();
			})
				.Produces<DeviceGroupType>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("GetDeviceGroupTypeByID")
				.WithDescription("Get a specific device group type by an ID")
				.WithSummary("By ID")
				.WithName("GetDeviceGroupTypeByID")
				;

			group.MapPut("Update", async (IDeviceGroupTypeRepository _repo, UpdateDeviceGroupTypeDTO update) =>
			{
				var result = await _repo.Update(new DeviceGroupType(update));

				return result.AsResponse();
			})
				.Produces<DeviceGroupType>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("UpdateDeviceGroupType")
				.WithDescription("Update a device group type")
				.WithSummary("Update")
				.WithName("UpdateDeviceGroupType")
				;

			group.MapDelete("Reactivate", async (IDeviceGroupTypeRepository _repo, long id) =>
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
				.Produces<DeviceGroupType>(200, "application/json")
				.Produces(404)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("ReactivateDeviceGroupType")
				.WithDescription("Reactivate a device group")
				.WithSummary("Reactivate")
				.WithName("ReactivateDeviceGroupType")
				;

			group.MapPost("Create", async (IDeviceGroupTypeRepository _repo, CreateDeviceGroupTypeDTO create) =>
			{
				var result = await _repo.Create(new DeviceGroupType(create));

				return result.AsResponse();
			})
				.Produces<DeviceGroupType>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateDeviceGroupType")
				.WithDescription("Create a device group type")
				.WithSummary("Create")
				.WithName("CreateDeviceGroupType")
				;

			group.MapPost("MQTT/Push", async (MqttService _mqtt, DeviceGroupTypePushMessage pushItem) =>
			{
				try
				{
					var message = new InjectedMqttApplicationMessage(
					   new MqttApplicationMessage
					   {
						   Topic = $"devicegrouptype/{pushItem.DeviceGroupTypeID}/commands",
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
				.WithDisplayName("DeviceGroupTypePush")
				.WithDescription("Push a message to a device group type. This is published to the topic 'devicegrouptype/{DeviceGroupTypeID}/commands'")
				.WithSummary("MQTT Push")
				.WithName("DeviceGroupTypePush")
				;

			group.MapDelete("Deactivate", async (IDeviceGroupTypeRepository _repo, long id) =>
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
				.Produces<DeviceGroupType>(200, "application/json")
				.Produces(404)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeactivateDeviceGroupType")
				.WithDescription("Deactivate a device group")
				.WithSummary("Deactivate")
				.WithName("DeactivateDeviceGroupType")
				;

			group.MapDelete("Delete", async (IDeviceGroupTypeRepository _repo, long id) =>
			{
				var result = await _repo.Delete(id);

				return result.AsResponse();
			})
				.Produces<DeviceGroupType>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteDeviceGroupType")
				.WithDescription("Perminently delete a device group type")
				.WithSummary("Delete")
				.WithName("DeleteDeviceGroupType")
				;

			return app;
		}
	}
}
