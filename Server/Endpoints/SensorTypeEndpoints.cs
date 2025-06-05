using Microsoft.AspNetCore.Mvc;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Server.Data.Models;
using Server.DTOs.SensorType;
using Server.Extensions;
using Server.Models.PushMessages;
using Server.Repositories;
using Server.Services;

namespace Server.Endpoints
{
	public static class SensorTypeEndpoints
	{
		public static WebApplication MapSensorTypeEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/SensorTypes").WithTags("Sensor Types");

			group.MapGet("", async (ISensorTypeRepository _repo, [FromQuery] bool ? activeOnly) =>
			{
				var result = !Convert.ToBoolean(activeOnly) ? await _repo.GetAll() : await _repo.GetWhere(x => x.DeactivatedOn == null);
				return result.AsResponse();
			})
				.Produces<List<SensorType>>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("SensorTypes")
				.WithDescription("Get all sensor types")
				.WithSummary("All")
				.WithName("SensorTypes")
				;

			group.MapGet("Details/{id}", async (ISensorTypeRepository _repo, string id) =>
			{
				var result = await _repo.GetById(id);

				return result.AsResponse();
			})
				.Produces<SensorType>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("GetSensorTypeByID")
				.WithDescription("Get a specific sensor type by an ID")
				.WithSummary("By ID")
				.WithName("GetSensorTypeByID")
				;

			group.MapPut("Update", async (ISensorTypeRepository _repo, UpdateSensorTypeDTO update) =>
			{
				var result = await _repo.Update(new SensorType(update));

				return result.AsResponse();
			})
				.Produces<SensorType>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("UpdateSensorType")
				.WithDescription("Update a sensor type")
				.WithSummary("Update")
				.WithName("UpdateSensorType")
				;

			group.MapPut("Reactivate", async (ISensorTypeRepository _repo, string id) =>
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
				.Produces<SensorType>(200, "application/json")
				.Produces(404)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("ReactivateSensorType")
				.WithDescription("Reactivate a sensor type")
				.WithSummary("Reactivate")
				.WithName("ReactivateSensorType")
				;

			group.MapPost("Create", async (ISensorTypeRepository _repo, CreateSensorTypeDTO create) =>
			{
				var result = await _repo.Create(new SensorType(create));

				return result.AsResponse();
			})
				.Produces<SensorType>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateSensorType")
				.WithDescription("Create a sensor type")
				.WithSummary("Create")
				.WithName("CreateSensorType")
				;

			group.MapPost("MQTT/Push", async (MqttService _mqtt, SensorTypePushMessage pushItem) =>
			{
				try
				{
					var message = new InjectedMqttApplicationMessage(
					   new MqttApplicationMessage
					   {
						   Topic = $"command/sensortype/{pushItem.SensorTypeID}",
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
				.WithDisplayName("SensorTypePush")
				.WithDescription("Push a message to a specific type of sensor. This is published to the topic 'command/sensortype/{SensorTypeID}'")
				.WithSummary("MQTT Push")
				.WithName("SensorTypePush")
				;

			group.MapDelete("Deactivate", async (ISensorTypeRepository _repo, string id) =>
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
				.Produces<SensorType>(200, "application/json")
				.Produces(404)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeactivateSensorType")
				.WithDescription("Deactivate a sensor type")
				.WithSummary("Deactivate")
				.WithName("DeactivateSensorType")
				;

			group.MapDelete("Delete/{id}", async (ISensorTypeRepository _repo, string id) =>
			{
				var result = await _repo.Delete(id);

				return result.AsResponse();
			})
				.Produces<SensorType>(200, "application/json")
				.Produces(404)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteSensorType")
				.WithDescription("Perminently delete a sensor type")
				.WithSummary("Delete")
				.WithName("DeleteSensorType")
				;

			return app;
		}
	}
}
