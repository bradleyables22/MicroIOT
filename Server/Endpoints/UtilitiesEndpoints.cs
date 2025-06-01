using Server.Data.Models;
using Server.DTOs.Device;
using Server.Repositories;

namespace Server.Endpoints
{
	public static class UtilitiesEndpoints
	{
		public static WebApplication MapUtilitiesEndpoints(this WebApplication app)
		{

			var group = app.MapGroup("api/v1/Utilities").WithTags("Utilities");

			group.MapGet("Sync/Clock",()=>Results.Ok(DateTime.UtcNow))
				.Produces<DateTime>(200, "text/plain")
				.WithDisplayName("SyncClock")
				.WithDescription("Allows a Device and/or sensor sync it's GPS clock to the server if necessary")
				.WithSummary("Sync Clock")
				.WithName("SyncClock")
				;
			group.MapGet("Generate/ID", () => Results.Ok(Guid.CreateVersion7().ToString()))
				.Produces<string>(200, "text/plain")
				.WithDisplayName("GenerateID")
				.WithDescription("Allows a Device request a unique ID if your architecture does not involve preflashed IDs.")
				.WithSummary("Generate ID")
				.WithName("GenerateID")
				;

			return app;
		}
	}
}
