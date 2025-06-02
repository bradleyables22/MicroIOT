using Server.Data.DTOs.OtaManifest;
using Server.Data.Models;
using Server.Extensions;
using Server.Repositories;

namespace Server.Endpoints
{
	public static class OtaManifestEndpoints
	{
		public static WebApplication MapOtaManifestEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/OTA").WithTags("OTA Manifest");

			group.MapGet("", async (IOtaManifestRepository _repo) =>
			{
				var result = await _repo.GetAll();
				return result.AsResponse();
			})
				.Produces<List<OtaManifestRecord>>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("FullManifest")
				.WithDescription("Get the full OTA manifest")
				.WithSummary("All")
			.WithName("FullManifest")
				;

			group.MapGet("Default/{deviceTypeID}", async (IOtaManifestRepository _repo, string deviceTypeID) =>
			{
				var result = await _repo.GetWhere(x => x.DeviceTypeID == deviceTypeID && x.Default == true);
				return result.AsResponse();
			})
				.Produces<OtaManifestRecord>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DefaultManifest")
				.WithDescription("Get the default OTA firmware for device type")
				.WithSummary("Get Default")
				.WithName("DefaultManifest")
				;

			group.MapGet("Devicetype/{id}", async (IOtaManifestRepository _repo, string id) =>
			{
				var result = await _repo.GetWhere(x=>x.DeviceTypeID == id);
				return result.AsResponse();
			})
				.Produces<List<OtaManifestRecord>>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("ManifestByDeviceType")
				.WithDescription("Get all manifest records for a given device type")
				.WithSummary("By Device Type")
				.WithName("ManifestByDeviceType")
				;

			group.MapGet("Details/{id}", async (IOtaManifestRepository _repo, long id) =>
			{
				var result = await _repo.GetById(id);

				return result.AsResponse();
			})
				.Produces<OtaManifestRecord>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("GetOtaRecordRecordByID")
				.WithDescription("Get a specific OTA manifest record by an ID")
				.WithSummary("By ID")
				.WithName("GetOtaRecordRecordByID")
				;

			group.MapPut("Update", async (IOtaManifestRepository _repo, UpdateOtaManifestDTO update) =>
			{

				var existingResult = await _repo.GetById(update.RecordID);
				if (existingResult.Success)
				{
					if (existingResult.Data == null)
						return Results.NotFound();
					else
					{
						existingResult.Data.DeviceTypeID = update.DeviceTypeID;
						existingResult.Data.Version = update.Version;
						existingResult.Data.Url = update.Url;
						existingResult.Data.Notes = update.Notes;

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
				.Produces<OtaManifestRecord>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("UpdateOtaRecord")
				.WithDescription("Update an OTA manifest record")
				.WithSummary("Update")
				.WithName("UpdateOtaRecord")
				;

			group.MapPut("Reactivate", async (IOtaManifestRepository _repo, long id) =>
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
				.Produces<OtaManifestRecord>(200, "application/json")
				.Produces(204)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("ReactivateOtaRecord")
				.WithDescription("Reactivate an ota record")
				.WithSummary("Reactivate")
				.WithName("ReactivateOtaRecord")
				;

			group.MapPut("Default", async (IOtaManifestRepository _repo, long id) =>
			{
				var existingResult = await _repo.GetById(id);
				if (existingResult.Success)
				{
					if (existingResult.Data == null)
						return Results.NotFound();

					if (existingResult.Data.Default == true)
						return Results.Problem(statusCode: 409,
							title: "Conflict",
							detail: "Currently Default"
							);
					else
					{
						existingResult.Data.Default = true;
						var UpdateResult = await _repo.Update(existingResult.Data);

						if (!UpdateResult.Success)
							return Results.Problem(statusCode: 500,
								title: "Exception",
								detail: UpdateResult.Exception?.Message);

						else 
						{
							var othersResult = await _repo.GetWhere(x => x.DeviceTypeID == existingResult.Data.DeviceTypeID, y => y.RecordID != existingResult.Data.RecordID);

							if (!othersResult.Success)
								return Results.Problem(statusCode: 500,
								title: "Exception",
								detail: othersResult.Exception?.Message);
							else 
							{
								if (othersResult.Data != null && othersResult.Data.Any()) 
								{
									foreach (var other in othersResult.Data)
									{
										other.Default = false;
										_ = await _repo.Update(other);
									}
								}
								return existingResult.AsResponse();
							}
						}
					}
				}
				else
				{
					return Results.Problem(statusCode: 500,
					title: "Exception",
					detail: existingResult.Exception?.Message);
				}
			})
				.Produces<OtaManifestRecord>(200, "application/json")
				.Produces(204)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DefaultOtaRecord")
				.WithDescription("Set an OTA manifest record to the default for a specific device type")
				.WithSummary("Set Default")
				.WithName("DefaultOtaRecord")
				;

			group.MapPost("Create", async (IOtaManifestRepository _repo, CreateOtaManifestDTO create) =>
			{
				var result = await _repo.Create(new OtaManifestRecord(create));

				return result.AsResponse();
			})
				.Produces<OtaManifestRecord>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("CreateOtaRecord")
				.WithDescription("Create an OTA manifest record")
				.WithSummary("Create")
				.WithName("CreateOtaRecord")
				;

			group.MapDelete("Deactivate", async (IOtaManifestRepository _repo, long id) =>
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
					else if(existingResult.Data.Default == true)
						return Results.Problem(statusCode: 409,
							title: "Conflict",
							detail: "Default records cannot be deactivated"
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
				.Produces<OtaManifestRecord>(200, "application/json")
				.Produces(204)
				.ProducesProblem(409, "application/json")
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeactivateOtaRecord")
				.WithDescription("Deactivate a ota record")
				.WithSummary("Deactivate")
				.WithName("DeactivateOtaRecord")
				;

			group.MapDelete("Delete", async (IOtaManifestRepository _repo, long id) =>
			{
				var existingResult = await _repo.GetById(id);
				if (existingResult.Success)
				{
					if (existingResult.Data == null)
						return Results.NotFound();

					if (existingResult.Data.Default == true)
						return Results.Problem(statusCode: 409,
							title: "Conflict",
							detail: "Default records cannot be deleted"
							);
					else
					{
						var result = await _repo.Delete(id);
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
				.Produces<OtaManifestRecord>(200, "application/json")
				.Produces(204)
				.ProducesProblem(500, "application/json")
				.WithDisplayName("DeleteOtaRecord")
				.WithDescription("Perminently delete a ota record, you cannot delete an OTA record that is set to default")
				.WithSummary("Delete")
				.WithName("DeleteOtaRecord")
				;

			return app;
		}

	}
}
