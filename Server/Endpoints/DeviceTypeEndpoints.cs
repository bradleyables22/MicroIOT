using Microsoft.AspNetCore.Mvc;
using Server.Data.Models;
using Server.DTOs.DeviceSensor;
using Server.DTOs.DeviceType;
using Server.Extensions;
using Server.Repositories;

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
				.Produces(204)
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
				.Produces(204)
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
				.Produces(204)
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
				.Produces(204)
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
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateDeviceType")
				.WithDescription("Create a device type")
				.WithSummary("Create")
				.WithName("CreateDeviceType")
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
				.Produces(204)
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
				.Produces(204)
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
