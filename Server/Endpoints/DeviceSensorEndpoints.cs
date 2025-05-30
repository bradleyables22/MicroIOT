using Server.Data.Models;
using Server.DTOs.DeviceGroup;
using Server.DTOs.DeviceSensor;
using Server.Extensions;
using Server.Repositories;
namespace Server.Endpoints
{
	public static class DeviceSensorEndpoints
	{
		public static WebApplication MapDeviceSensorEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/Sensors").WithTags("Device Sensors");

			group.MapGet("{deviceID}", async (IDeviceSensorRepository _repo, string deviceID) =>
			{
				var result = await _repo.GetWhere(x => x.DeviceID == deviceID);
				return result.AsResponse();
			})
				.Produces<List<DeviceSensor>>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("SensorsByDevice")
				.WithDescription("Get all sensors belonging to a device")
				.WithSummary("By Device")
				.WithName("SensorsBydevice")
				;

			group.MapGet("Details/{id}", async (IDeviceSensorRepository _repo, string id) =>
			{
				var result = await _repo.GetById(id);

				return result.AsResponse();
			})
				.Produces<DeviceSensor>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("SensorByID")
				.WithDescription("Get a specific device sensor by an ID")
				.WithSummary("By ID")
				.WithName("SensorByID")
				;

			group.MapPut("Update", async (IDeviceSensorRepository _repo, UpdateDeviceSensorDTO update) =>
			{
				var result = await _repo.Update(new DeviceSensor(update));

				return result.AsResponse();
			})
				.Produces<DeviceSensor>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("UpdateDeviceSensor")
				.WithDescription("Update a device sensor")
				.WithSummary("Update")
				.WithName("UpdateDeviceSensor")
				;
			group.MapPut("Reactivate", async (IDeviceSensorRepository _repo, string id) =>
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
				.Produces<DeviceSensor>(200, "application/json")
				.Produces(204)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("ReactivateDeviceSensor")
				.WithDescription("Reactivate a device sensor")
				.WithSummary("Reactivate")
				.WithName("ReactivateDeviceSensor")
				;

			group.MapPost("Create", async (IDeviceSensorRepository _repo, CreateDeviceSensorDTO create) =>
			{
				var result = await _repo.Create(new DeviceSensor(create));

				return result.AsResponse();
			})
				.Produces<DeviceSensor>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateDeviceSensor")
				.WithDescription("Create a device sensor")
				.WithSummary("Create")
				.WithName("CreateDeviceSensor")
				;

			group.MapDelete("Deactivate", async (IDeviceSensorRepository _repo, string id) =>
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
				.Produces<DeviceSensor>(200, "application/json")
				.Produces(204)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeactivateDeviceSensor")
				.WithDescription("Deactivate a device sensor")
				.WithSummary("Deactivate")
				.WithName("DeactivateDeviceSensor")
				;

			group.MapDelete("Delete", async (IDeviceSensorRepository _repo, string id) =>
			{
				var result = await _repo.Delete(id);

				return result.AsResponse();
			})
				.Produces<DeviceSensor>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteDeviceSensor")
				.WithDescription("Perminently Delete a device sensor")
				.WithSummary("Delete")
				.WithName("DeleteDeviceSensor")
				;

			return app;
		}
	}
}
