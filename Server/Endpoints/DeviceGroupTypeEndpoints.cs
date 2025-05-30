using Server.Data.Models;
using Server.DTOs.DeviceSensor;
using Server.Extensions;
using Server.Repositories;

namespace Server.Endpoints
{
	public static class DeviceGroupTypeEndpoints
	{
		public static WebApplication MapDeviceGroupTypeEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/DeviceGroupTypes").WithTags("Device Group Types");

			group.MapGet("", async (IDeviceGroupTypeRepository _repo) =>
			{
				var result = await _repo.GetAll();
				return result.AsResponse();
			})
				.Produces<List<DeviceGroupType>>(200, "application/json")
				.Produces(204)
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
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("GetDeviceGroupTypeByID")
				.WithDescription("Get a specific device group type by a specific ID")
				.WithSummary("By ID")
				.WithName("GetDeviceGroupTypeByID")
				;

			group.MapPut("Update", async (IDeviceGroupTypeRepository _repo, UpdateDeviceGroupTypeDTO update) =>
			{
				var result = await _repo.Update(new DeviceGroupType(update));

				return result.AsResponse();
			})
				.Produces<DeviceGroupType>(200, "application/json")
				.Produces(204)
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
				.Produces(204)
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
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateDeviceGroupType")
				.WithDescription("Create a device group type")
				.WithSummary("Create")
				.WithName("CreateDeviceGroupType")
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
				.Produces(204)
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
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteDeviceGroupType")
				.WithDescription("Perminently Delete a device group type")
				.WithSummary("Delete")
				.WithName("DeleteDeviceGroupType")
				;

			return app;
		}
	}
}
