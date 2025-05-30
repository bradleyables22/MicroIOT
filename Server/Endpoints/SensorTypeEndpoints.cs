using Server.Data.Models;
using Server.DTOs.SensorType;
using Server.Extensions;
using Server.Repositories;

namespace Server.Endpoints
{
	public static class SensorTypeEndpoints
	{
		public static WebApplication MapSensorTypeEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/SensorTypes").WithTags("Sensor Types");

			group.MapGet("", async (ISensorTypeRepository _repo) =>
			{
				var result = await _repo.GetAll();
				return result.AsResponse();
			})
				.Produces<List<SensorType>>(200, "application/json")
				.Produces(204)
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
				.Produces(204)
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
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("UpdateDeviceType")
				.WithDescription("Update a sensor type")
				.WithSummary("Update")
				.WithName("UpdateSensorType")
				;

			group.MapPost("Create", async (ISensorTypeRepository _repo, CreateSensorTypeDTO create) =>
			{
				var result = await _repo.Create(new SensorType(create));

				return result.AsResponse();
			})
				.Produces<SensorType>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateSensorType")
				.WithDescription("Create a sensor type")
				.WithSummary("Create")
				.WithName("CreateSensorType")
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
				.Produces(204)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeactivateSensorType")
				.WithDescription("Deactivate a sensor type")
				.WithSummary("Deactivate")
				.WithName("DeactivateSensorType")
				;

			group.MapDelete("Delete", async (ISensorTypeRepository _repo, string id) =>
			{
				var result = await _repo.Delete(id);

				return result.AsResponse();
			})
				.Produces<SensorType>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteSensorType")
				.WithDescription("Perminently Delete a sensor type")
				.WithSummary("Delete")
				.WithName("DeleteSensorType")
				;

			return app;
		}
	}
}
