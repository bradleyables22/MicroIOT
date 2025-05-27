using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Services.Background;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddOpenApi();

builder.Services.AddHostedService<DummyDataService>();

var app = builder.Build();

app.MapOpenApi();
app.UseHttpsRedirection();

app.Run();

