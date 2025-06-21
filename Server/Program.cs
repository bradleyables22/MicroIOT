using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using MQTTnet.Server;
using Scalar.AspNetCore;
using Server.Components;
using Server.Data;
using Server.Endpoints;
using Server.Repositories.Extensions;
using Server.Services;
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
builder.Services.AddSingleton<MqttService>();
builder.Services.AddSingleton<DeviceTracker>();
builder.Services.AddScoped<ToastService>();
//builder.Services.AddHostedService<MqttBackgroundService>();

builder.Services.AddCors(options => 
{
	options.AddPolicy("allow-all", builder =>
	{
		builder.AllowAnyOrigin();
		builder.AllowAnyMethod();
		builder.AllowAnyHeader();
	});
});

builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();


var app = builder.Build();

app.UseCors("allow-all");
app.MapOpenApi();
app.UseHttpsRedirection();
app.UseAntiforgery();
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

app.UseStaticFiles();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();
app.Run();

