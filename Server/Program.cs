using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Graphql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddOpenApi();

builder.Services.AddGraphQLServer()
	.AddQueryType<Query>()
	.ModifyCostOptions(_=>_.SkipAnalyzer = true)
	.AddFiltering()
	.AddSorting()
	.AddProjections();

var app = builder.Build();

app.MapOpenApi();
app.MapGraphQL("/graphql");
app.UseHttpsRedirection();

app.Run();

