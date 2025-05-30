using Server.Data.Models;
using Server.DTOs.SensorCategory;
using Server.DTOs.SensorType;
using Server.Extensions;
using Server.Repositories;

namespace Server.Endpoints
{
	public static class SensorCategoryEndpoints
	{
		public static WebApplication MapSensorCategoryEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/SensorCategories").WithTags("Sensor Categories");

			group.MapGet("", async (ISensorCategoryRepository _repo) =>
			{
				var result = await _repo.GetAll();
				return result.AsResponse();
			})
				.Produces<List<SensorCategory>>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("SensorCategories")
				.WithDescription("Get all sensor categories")
				.WithSummary("All")
				.WithName("SensorCategories")
				;

			group.MapGet("Details/{id}", async (ISensorCategoryRepository _repo, long id) =>
			{
				var result = await _repo.GetById(id);

				return result.AsResponse();
			})
				.Produces<SensorCategory>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("GetSensorCategoryByID")
				.WithDescription("Get a specific sensor category by an ID")
				.WithSummary("By ID")
				.WithName("GetSensorCategoryByID")
				;

			group.MapPut("Update", async (ISensorCategoryRepository _repo, UpdateSensorCategoryDTO update) =>
			{
				var result = await _repo.Update(new SensorCategory(update));

				return result.AsResponse();
			})
				.Produces<SensorCategory>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("UpdateDeviceType")
				.WithDescription("Update a sensor category")
				.WithSummary("Update")
				.WithName("UpdateSensorType")
				;

			group.MapPost("Create", async (ISensorCategoryRepository _repo, CreateSensorCategoryDTO create) =>
			{
				var result = await _repo.Create(new SensorCategory(create));

				return result.AsResponse();
			})
				.Produces<SensorCategory>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateSensorCategory")
				.WithDescription("Create a sensor category")
				.WithSummary("Create")
				.WithName("CreateSensorCategory")
				;

			group.MapDelete("Deactivate", async (ISensorCategoryRepository _repo, long id) =>
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
				.Produces<SensorCategory>(200, "application/json")
				.Produces(204)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeactivateSensorCategory")
				.WithDescription("Deactivate a sensor category")
				.WithSummary("Deactivate")
				.WithName("DeactivateSensorCategory")
				;

			group.MapDelete("Delete", async (ISensorCategoryRepository _repo, long id) =>
			{
				var result = await _repo.Delete(id);

				return result.AsResponse();
			})
				.Produces<SensorCategory>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteSensorCategory")
				.WithDescription("Perminently Delete a sensor category")
				.WithSummary("Delete")
				.WithName("DeleteSensorCategory")
				;

			return app;
		}
	}
}
