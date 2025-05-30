using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Server.Data;
using Server.Endpoints;
using Server.Repositories.Extensions;
using Server.Services.Background;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddRepositories();

builder.Services.AddOpenApi("v1");

builder.Services.AddHostedService<DummyDataService>();

var app = builder.Build();

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
app.Run();

