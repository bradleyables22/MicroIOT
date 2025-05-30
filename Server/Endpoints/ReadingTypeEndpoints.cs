using Server.Data.Models;
using Server.DTOs.DeviceType;
using Server.DTOs.ReadingType;
using Server.Extensions;
using Server.Repositories;

namespace Server.Endpoints
{
	public static class ReadingTypeEndpoints
	{
		public static WebApplication MapReadingTypeEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/ReadingTypes").WithTags("Reading Types");

			group.MapGet("", async (IReadingTypeRepository _repo) =>
			{
				var result = await _repo.GetAll();
				return result.AsResponse();
			})
				.Produces<List<ReadingType>>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("ReadingTypes")
				.WithDescription("Get all reading types")
				.WithSummary("All")
				.WithName("ReadingTypes")
				;

			group.MapGet("Details/{id}", async (IReadingTypeRepository _repo, string id) =>
			{
				var result = await _repo.GetById(id);

				return result.AsResponse();
			})
				.Produces<ReadingType>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("GetReadingTypeByID")
				.WithDescription("Get a specific reading type by an ID")
				.WithSummary("By ID")
				.WithName("GetReadingTypeByID")
				;

			group.MapPut("Update", async (IReadingTypeRepository _repo, UpdateReadingTypeDTO update) =>
			{
				var result = await _repo.Update(new ReadingType(update));

				return result.AsResponse();
			})
				.Produces<ReadingType>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("UpdateReadingType")
				.WithDescription("Update a reading type")
				.WithSummary("Update")
				.WithName("UpdateReadingType")
				;

			group.MapPost("Create", async (IDeviceTypeRepository _repo, CreateDeviceTypeDTO create) =>
			{
				var result = await _repo.Create(new DeviceType(create));

				return result.AsResponse();
			})
				.Produces<ReadingType>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateReadingType")
				.WithDescription("Create a reading type")
				.WithSummary("Create")
				.WithName("CreateReadingType")
				;

			group.MapDelete("Deactivate", async (IReadingTypeRepository _repo, string id) =>
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
				.Produces<ReadingType>(200, "application/json")
				.Produces(204)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeactivateReadingType")
				.WithDescription("Deactivate a reading type")
				.WithSummary("Deactivate")
				.WithName("DeactivateReadingType")
				;

			group.MapDelete("Delete", async (IReadingTypeRepository _repo, string id) =>
			{
				var result = await _repo.Delete(id);

				return result.AsResponse();
			})
				.Produces<ReadingType>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteReadingType")
				.WithDescription("Perminently Delete a reading type")
				.WithSummary("Delete")
				.WithName("DeleteReadingType")
				;

			return app;
		}
	}
}
