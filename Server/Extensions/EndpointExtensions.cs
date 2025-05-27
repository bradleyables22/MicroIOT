using Server.Data.Models;
using Server.Repositories.Models;

namespace Server.Extensions
{
	public static class EndpointExtensions
	{
		public static IResult GetResult<T>(this RepositoryResponse<T> response) where T : BaseModel 
		{
			try
			{
				if (response.Success) 
				{
					if (response.Data == null)
						return Results.NotFound();
					return Results.Ok(response.Data);
				}

				return Results.Problem(statusCode: 500,
					title: "Exception",
					detail: response.Exception?.Message);
			}
			catch (Exception e)
			{
				return Results.Problem(statusCode: 500,
					title: "Exception",
					detail: e.Message);
			}
		}

	}
}
