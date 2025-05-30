﻿using Microsoft.AspNetCore.Mvc;
using Server.Data.Models;
using Server.DTOs.DeviceGroup;
using Server.DTOs.SystemGroup;
using Server.Extensions;
using Server.Repositories;

namespace Server.Endpoints
{
	public static class DeviceGroupEndpoints
	{
		public static WebApplication MapDeviceGroupEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/DeviceGroups").WithTags("Device Groups");

			group.MapGet("{systemGroupID}", async (IDeviceGroupRepository _repo, long systemGroupID) =>
			{
				var result = await _repo.GetWhere(x => x.SystemGroupID == systemGroupID);
				return result.AsResponse();
			})
				.Produces<List<DeviceGroup>>(200, "application/json")
				.Produces(204)
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
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("GetDeviceGroupByID")
				.WithDescription("Get a specific device group by a specific ID")
				.WithSummary("By ID")
				.WithName("GetDeviceGroupByID")
				;

			group.MapPut("Update", async (IDeviceGroupRepository _repo, UpdateDeviceGroupDTO update) =>
			{
				var result = await _repo.Update(new DeviceGroup(update));

				return result.AsResponse();
			})
				.Produces<DeviceGroup>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("UpdateDeviceGroup")
				.WithDescription("Update a device group")
				.WithSummary("Update")
				.WithName("UpdateDeviceGroup")
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
				.Produces(204)
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
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateDeviceGroup")
				.WithDescription("Create a device group")
				.WithSummary("Create")
				.WithName("CreateDeviceGroup")
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
				.Produces(204)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeactivateDeviceGroup")
				.WithDescription("Deactivate a device group")
				.WithSummary("Deactivate")
				.WithName("DeactivateDeviceGroup")
				;

			group.MapDelete("Delete", async (IDeviceGroupRepository _repo, string id) =>
			{
				var result = await _repo.Delete(id);

				return result.AsResponse();
			})
				.Produces<DeviceGroup>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteDeviceGroup")
				.WithDescription("Perminently Delete a device group")
				.WithSummary("Delete")
				.WithName("DeleteDeviceGroup")
				;

			return app;
		}
	}
}
