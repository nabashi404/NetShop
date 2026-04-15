using NetShop;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppConfiguration(builder.Configuration);
builder.Services.AddAppAuthentication(builder.Configuration);
builder.Services.AddAppDatabase(builder.Configuration);
builder.Services.AddAppIdentity();
builder.Services.AddAppServices();
builder.Services.AddAppBlazor();

var app = builder.Build();

app.UseAppPipeline();

app.MapAppEndpoints();

await SeedData.InitializeAsync(app.Services);

app.Run();
