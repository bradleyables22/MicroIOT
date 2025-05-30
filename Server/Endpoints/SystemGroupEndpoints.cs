using Microsoft.AspNetCore.Mvc;
using Server.Data.Models;
using Server.DTOs.SystemGroup;
using Server.Extensions;
using Server.Repositories;

namespace Server.Endpoints
{
	public static class SystemGroupEndpoints
	{
		public static WebApplication MapSystemGroupEndpoints(this WebApplication app) 
		{

			var group = app.MapGroup("api/v1/SystemGroups").WithTags("System Groups");

			group.MapGet("",async (ISystemGroupRepository _repo, [FromQuery] string? name) =>
			{
				var result = string.IsNullOrEmpty(name) ? await _repo.GetAll():await _repo.GetWhere(x=>x.Name.ToLower().Contains(name.ToLower()));
				return result.AsResponse();
			})
				.Produces<List<SystemGroup>>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500,"application/json")
				.WithDisplayName("SearchSystems")
				.WithDescription("Get all system groups")
				.WithSummary("Search")
				.WithName("SearchSystems")
				;

			group.MapGet("Details/{id}", async (ISystemGroupRepository _repo, int id) =>
			{
				var result = await _repo.GetById(id);

				return result.AsResponse();
			})
				.Produces<SystemGroup>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("GetSystemByID")
				.WithDescription("Get a system group by a specific ID")
				.WithSummary("By ID")
				.WithName("GetSystemByID")
				;

			group.MapPut("Update", async (ISystemGroupRepository _repo, UpdateSystemGroupDTO update) =>
			{
				var result = await _repo.Update(new SystemGroup(update));

				return result.AsResponse();
			})
				.Produces<SystemGroup>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("UpdateSystemGroup")
				.WithDescription("Update a system group")
				.WithSummary("Update")
				.WithName("UpdateSystem")
				;

			group.MapPost("Create", async (ISystemGroupRepository _repo, CreateSystemGroupDTO create) =>
			{
				var result = await _repo.Create(new SystemGroup(create));

				return result.AsResponse();
			})
				.Produces<SystemGroup>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateSystem")
				.WithDescription("Create a system group")
				.WithSummary("Create")
				.WithName("CreateSystem")
				;

			group.MapDelete("Deactivate", async (ISystemGroupRepository _repo, long id) =>
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
				.Produces<SystemGroup>(200, "application/json")
				.Produces(204)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeactivateSystem")
				.WithDescription("Deactivate a system group")
				.WithSummary("Deactivate")
				.WithName("DeactivateSystem")
				;

			group.MapDelete("Delete", async (ISystemGroupRepository _repo, long id) =>
			{
				var result = await _repo.Delete(id);

				return result.AsResponse();
			})
				.Produces<SystemGroup>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteSystem")
				.WithDescription("Perminently Delete a system group")
				.WithSummary("Delete")
				.WithName("DeleteSystem")
				;

			return app;
		}
	}
}
