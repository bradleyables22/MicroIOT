using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Server.Data;
using Server.Endpoints;
using Server.Repositories.Extensions;
using Server.Services.Background;
using Server.Transformers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddRepositories();

builder.Services.AddOpenApi("v1", options => 
{
	options.AddDocumentTransformer<TitleTransformer>();
});

builder.Services.AddHostedService<DummyDataService>();

builder.Services.AddCors(options => 
{
	options.AddPolicy("allow-all", builder =>
	{
		builder.AllowAnyOrigin();
		builder.AllowAnyMethod();
		builder.AllowAnyHeader();
	});
});

var app = builder.Build();

var rewrite = new RewriteOptions()
	.AddRewrite(@"^(?!swagger|scalar|api|metrics)([a-zA-Z0-9/_-]+)$", "$1.html", skipRemainingRules: true)
	.AddRewrite(@"^(?!swagger|scalar|api|metrics)pages/([a-zA-Z0-9/_-]+)$", "pages/$1.html", skipRemainingRules: true);

app.UseRewriter(rewrite);
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("allow-all");
app.MapOpenApi();
app.UseHttpsRedirection();
app.MapScalarApiReference(options => 
{
	options.Title = "Micro IOT";
	options.Theme = ScalarTheme.BluePlanet;
	options.ForceThemeMode = ThemeMode.Dark;
	options.WithDarkModeToggle(false);
});

app.MapSystemGroupEndpoints();
app.MapDeviceGroupEndpoints();
app.MapDeviceGroupTypeEndpoints();
app.MapDeviceEndpoints();
app.MapDeviceTypeEndpoints();
app.MapDeviceSensorEndpoints();
app.MapSensorTypeEndpoints();
app.MapSensorCategoryEndpoints();
app.MapReadingTypeEndpoints();
app.MapReadingEndpoints();
app.MapUtilitiesEndpoints();
app.MapOtaManifestEndpoints();
app.MapOtaOverridesEndpoints();
app.MapOtaDownloadEndpoints();
app.Run();

