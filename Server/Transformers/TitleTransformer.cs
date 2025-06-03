using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Server.Transformers
{
	public class TitleTransformer : IOpenApiDocumentTransformer
	{
		public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
		{
			document.Info.Title = "Micro IOT API";
			document.Info.Description = "Welcome to your IOT integration hub rest API documentation.";
			return Task.FromResult(document);
		}
	}
}
