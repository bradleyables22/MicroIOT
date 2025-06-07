using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Data.Models;
using Server.DTOs.Device;
using Server.DTOs.Reading;
using Server.Extensions;
using Server.Models;
using Server.Repositories;

namespace Server.Endpoints
{
	public static class ReadingEndpoints
	{
		public static WebApplication MapReadingEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/Readings").WithTags("Readings");

			group.MapGet("{sensorID}", async (AppDbContext _context,string sensorID,[FromQuery] long? afterReadingID,[FromQuery] int take = 10) =>
			{
				if (take <= 0 || take > 1000)
					return Results.Problem("Invalid 'take' parameter. Must be between 1 and 1000.");

				var query = _context.SensorReadings
					.Where(x => x.SensorID == sensorID);

				if (afterReadingID.HasValue)
					query = query.Where(x => x.ReadingID < afterReadingID);

				var results = await query
					.OrderByDescending(x => x.ReadingID)
					.Take(take)
					.ToListAsync();

				var count = results?.Count ?? 0;

				return Results.Ok(new PagedResponse<SensorReading>
				{
					Count = count,
					Last = results?.LastOrDefault()?.ReadingID,
					Data = results,
					End =  count<take
				});
			})
			.Produces<PagedResponse<SensorReading>>(200, "application/json")
			.ProducesProblem(400, "application/json")
			.ProducesProblem(500, "application/json")
			.WithName("GetSensorReadings")
			.WithSummary("By Sensor")
			.WithDescription("Paginate sensor readings efficiently using ReadingID as the cursor.");

			group.MapGet("Details/{id}", async (ISensorReadingRepository _repo, long id) =>
			{
				var result = await _repo.GetById(id);

				return result.AsResponse();
			})
				.Produces<SensorReading>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("GetReadingByID")
				.WithDescription("Get a specific sensor reading by an ID")
				.WithSummary("By ID")
				.WithName("GetReadingByID")
				;

			group.MapPut("Update", async (ISensorReadingRepository _repo, UpdateSensorReadingDTO update) =>
			{
				var existingResult = await _repo.GetById(update.ReadingID);
				if (existingResult.Success)
				{
					if (existingResult.Data == null)
						return Results.NotFound();

					existingResult.Data.SensorID = update.SensorID;
					existingResult.Data.Value = update.Value;
					existingResult.Data.Metadata = update.Metadata;
					existingResult.Data.ReadingTypeID = update.ReadingTypeID;

					var result = await _repo.Update(existingResult.Data);
					return result.AsResponse();
				}
				else
				{
					return Results.Problem(statusCode: 500,
					title: "Exception",
					detail: existingResult.Exception?.Message);
				}
			})
				.Produces<SensorReading>(200, "application/json")
				.Produces(404)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("UpdateReading")
				.WithDescription("Update a sensor reading. This is also available via MQTT by posting on 'readings/update'. Device Group ID should be used for clientId.")
				.WithSummary("Update")
				.WithName("UpdateReading")
				;
			
			group.MapPost("Create", async (ISensorReadingRepository _repo, CreateSensorReadingDTO create) =>
			{
				var result = await _repo.Create(new SensorReading(create));

				return result.AsResponse();
			})
				.Produces<SensorReading>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateReading")
				.WithDescription("Create a sensor reading. This is also available via MQTT by posting on 'readings/create'. Device Group ID should be used for clientId.")
				.WithSummary("Create")
				.WithName("CreateReading")
				;

			group.MapDelete("Delete/{id}", async (ISensorReadingRepository _repo, long id) =>
			{
				var result = await _repo.Delete(id);

				return result.AsResponse();
			})
				.Produces<SensorReading>(200, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteReading")
				.WithDescription("Perminently delete a sensor reading.")
				.WithSummary("Delete")
				.WithName("DeleteReading")
				;

			return app;
		}
	}
}
