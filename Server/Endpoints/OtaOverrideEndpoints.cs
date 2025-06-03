using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Server.Data.DTOs.OtaManifest;
using Server.Data.DTOs.OtaOverride;
using Server.Data.Models;
using Server.Extensions;
using Server.Repositories;

namespace Server.Endpoints
{
	public static class OtaOverrideEndpoints
	{
		public static WebApplication MapOtaOverridesEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/Overrides").WithTags("OTA Overrides");

			group.MapGet("", async (IOtaOverrideRepository _repo) =>
			{
				var result = await _repo.GetAll();
				return result.AsResponse();
			})
				.Produces<List<OtaOverrideRecord>>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("AllOverrides")
				.WithDescription("Get all device overrides")
				.WithSummary("All")
			.WithName("AllOverrides")
				;

			group.MapGet("Find/{deviceID}", async (IOtaOverrideRepository _repo, string deviceID) =>
			{
				var result = await _repo.GetById(deviceID);
				return result.AsResponse();
			})
				.Produces<OtaOverrideRecord>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("OverrideByDevice")
				.WithDescription("Check to see if the given device has a registered OTA override it should use.")
				.WithSummary("By Device")
				.WithName("OverrideByDevice")
				;


			group.MapPut("Update", async (IOtaOverrideRepository _repo, UpdateOtaOverrideDTO update) =>
			{
				var result = await _repo.Update(new OtaOverrideRecord(update));
			})
				.Produces<OtaOverrideRecord>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("UpdateOverrideRecord")
				.WithDescription("Update an OTA override record")
				.WithSummary("Update")
				.WithName("UpdateOverrideRecord")
				;

			group.MapPost("Create", async (IOtaOverrideRepository _repo, CreateOtaOverrideDTO create) =>
			{
				var result = await _repo.Create(new OtaOverrideRecord(create));

				return result.AsResponse();
			})
				.Produces<OtaOverrideRecord>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateOverrideRecord")
				.WithDescription("Create an OTA override record for a device")
				.WithSummary("Create")
				.WithName("CreateOverrideRecord")
				;

			group.MapDelete("Delete", async (IOtaOverrideRepository _repo, long id) =>
			{
				var result = await _repo.Delete(id);
			})
				.Produces<OtaOverrideRecord>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteOverrideRecord")
				.WithDescription("Perminently delete an override")
				.WithSummary("Delete")
				.WithName("DeleteOverrideRecord")
				;

			return app;
		}
	}
}
