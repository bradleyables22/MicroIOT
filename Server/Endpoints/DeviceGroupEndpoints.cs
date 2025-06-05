using Microsoft.AspNetCore.Mvc;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Server.Data.Models;
using Server.DTOs.DeviceGroup;
using Server.DTOs.SystemGroup;
using Server.Extensions;
using Server.Models.PushMessages;
using Server.Repositories;
using Server.Services;
using System.Text;

namespace Server.Endpoints
{
	public static class DeviceGroupEndpoints
	{
		public static WebApplication MapDeviceGroupEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/DeviceGroups").WithTags("Device Groups");

			group.MapGet("{systemGroupID}", async (IDeviceGroupRepository _repo, long systemGroupID, [FromQuery] bool? activeOnly) =>
			{
				var result = !Convert.ToBoolean(activeOnly) ? await _repo.GetAll() : await _repo.GetWhere(x => x.DeactivatedOn == null);
				return result.AsResponse();
			})
				.Produces<List<DeviceGroup>>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeviceGroupsBySystem")
				.WithDescription("Get all device groups belonging to a system group")
				.WithSummary("By System")
				.WithName("DeviceGroupsBySystem")
				;

			group.MapGet("Details/{id}", async (IDeviceGroupRepository _repo, string id) =>
			{
				var result = await _repo.GetById(id);

				return result.AsResponse();
			})
				.Produces<DeviceGroup>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("GetDeviceGroupByID")
				.WithDescription("Get a specific device group by an ID")
				.WithSummary("By ID")
				.WithName("GetDeviceGroupByID")
				;

			group.MapPut("Update", async (IDeviceGroupRepository _repo, UpdateDeviceGroupDTO update) =>
			{
				var result = await _repo.Update(new DeviceGroup(update));

				return result.AsResponse();
			})
				.Produces<DeviceGroup>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("UpdateDeviceGroup")
				.WithDescription("Update a device group")
				.WithSummary("Update")
				.WithName("UpdateDeviceGroup")
				;

			group.MapPut("Reactivate", async (IDeviceGroupRepository _repo, string id) =>
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
				.Produces<DeviceGroup>(200, "application/json")
				.Produces(404)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("ReactivateDeviceGroup")
				.WithDescription("Reactivate a device group")
				.WithSummary("Reactivate")
				.WithName("ReactivateDeviceGroup")
				;

			group.MapPut("Confirm", async (IDeviceGroupRepository _repo, string id) =>
			{
				var existingResult = await _repo.GetById(id);
				if (existingResult.Success)
				{
					if (existingResult.Data == null)
						return Results.NotFound();

					if (existingResult.Data.ConfirmedOn != null)
						return Results.Problem(statusCode: 409,
							title: "Conflict",
							detail: "Already confirmed"
							);
					else
					{
						existingResult.Data.ConfirmedOn = DateTime.UtcNow;
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
				.Produces<DeviceGroup>(200, "application/json")
				.Produces(404)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("ConfirmDeviceGroup")
				.WithDescription("Confirm a device group")
				.WithSummary("Confirm")
				.WithName("ConfirmDeviceGroup")
				;

			group.MapPost("Create", async (IDeviceGroupRepository _repo, CreateDeviceGroupDTO create) =>
			{
				var result = await _repo.Create(new DeviceGroup(create));

				return result.AsResponse();
			})
				.Produces<DeviceGroup>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateDeviceGroup")
				.WithDescription("Create a device group")
				.WithSummary("Create")
				.WithName("CreateDeviceGroup")
				;

			group.MapPost("MQTT/Push", async (MqttService _mqtt, DeviceGroupPushMessage pushItem) =>
			{
				try
				{
					var message = new InjectedMqttApplicationMessage(
					   new MqttApplicationMessage
					   {
						   Topic = $"devicegroup/{pushItem.DeviceGroupID}/commands",
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
				.WithDisplayName("DeviceGroupPush")
				.WithDescription("Push a message to a device group. This is published to the topic 'devicegroup/{DeviceGroupID}/commands'")
				.WithSummary("MQTT Push")
				.WithName("DeviceGroupPush")
				;

			group.MapDelete("Deactivate", async (IDeviceGroupRepository _repo, string id) =>
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
				.Produces<DeviceGroup>(200, "application/json")
				.Produces(404)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeactivateDeviceGroup")
				.WithDescription("Deactivate a device group")
				.WithSummary("Deactivate")
				.WithName("DeactivateDeviceGroup")
				;

			group.MapDelete("Delete/{id}", async (IDeviceGroupRepository _repo, string id) =>
			{
				var result = await _repo.Delete(id);

				return result.AsResponse();
			})
				.Produces<DeviceGroup>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteDeviceGroup")
				.WithDescription("Perminently delete a device group")
				.WithSummary("Delete")
				.WithName("DeleteDeviceGroup")
				;

			return app;
		}
	}
}
