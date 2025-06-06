using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.DTOs.OtaDownload;
using Server.Data.Models;
using Server.DTOs.Reading;
using Server.Extensions;
using Server.Models;
using Server.Repositories;

namespace Server.Endpoints
{
	public static class OtaDownloadEndpoints
	{
		public static WebApplication MapOtaDownloadEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/Downloads").WithTags("OTA Downloads");

			group.MapGet("{deviceID}", async (AppDbContext _context, string deviceID, [FromQuery] long? afterRecordID, [FromQuery] int take = 10) =>
			{
				if (take <= 0 || take > 1000)
					return Results.Problem("Invalid 'take' parameter. Must be between 1 and 1000.");

				var query = _context.OTA_Downloads
					.Where(x => x.DeviceID == deviceID);

				if (afterRecordID.HasValue)
					query = query.Where(x => x.RecordID < afterRecordID);

				var results = await query
					.OrderByDescending(x => x.RecordID)
					.Take(take)
					.ToListAsync();

				if (!results.Any())
					return Results.NoContent();

				var count = results?.Count ?? 0;

				return Results.Ok(new PagedResponse<OtaDownloadRecord>
				{
					Count = count,
					Last = results?.LastOrDefault()?.RecordID,
					Data = results,
					End = count < take
				});
			})
			.Produces<PagedResponse<OtaDownloadRecord>>(200, "application/json")
			.ProducesProblem(400, "application/json")
			.ProducesProblem(500, "application/json")
			.WithName("GetDownloadsByDevice")
			.WithSummary("By Device")
			.WithDescription("Paginate download history for a device efficiently using DeviceID as the cursor.");

			group.MapGet("Details/{id}", async (IOtaDownloadRepository _repo, long id) =>
			{
				var result = await _repo.GetById(id);

				return result.AsResponse();
			})
				.Produces<OtaDownloadRecord>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("GetDownloadByID")
				.WithDescription("Get a specific download record by an ID")
				.WithSummary("By ID")
				.WithName("GetDownloadByID")
				;

			group.MapPut("Update", async (IOtaDownloadRepository _repo, UpdateOtaDownloadRecordDTO update) =>
			{
				var result = await _repo.Update(new OtaDownloadRecord(update));

				return result.AsResponse();
			})
				.Produces<OtaDownloadRecord>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("UpdateDownloadRecord")
				.WithDescription("Update an OTA download record. This is also available via MQTT by posting on 'downloads/update'. Device Group ID should be used for clientId as well.")
				.WithSummary("Update")
				.WithName("UpdateDownloadRecord")
				;

			group.MapPost("Create", async (IOtaDownloadRepository _repo, CreateOtaDownloadRecordDTO create) =>
			{
				var result = await _repo.Create(new OtaDownloadRecord(create));

				return result.AsResponse();
			})
				.Produces<OtaDownloadRecord>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateDownloadRecord")
				.WithDescription("Create an OTA download record. This is also available via MQTT by posting on 'downloads/create'. Device Group ID should be used for clientId as well.")
				.WithSummary("Create")
				.WithName("CreateDownloadRecord")
				;

			group.MapDelete("Delete", async (IOtaDownloadRepository _repo, long id) =>
			{
				var result = await _repo.Delete(id);

				return result.AsResponse();
			})
				.Produces<OtaDownloadRecord>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteDownloadRecord")
				.WithDescription("Perminently delete an OTA download record. This is also available via MQTT by posting on 'downloads/delete/{id}'. Device Group ID should be used for clientId as well.")
				.WithSummary("Delete")
				.WithName("DeleteDownloadRecord")
				;

			return app;
		}
	}
}
