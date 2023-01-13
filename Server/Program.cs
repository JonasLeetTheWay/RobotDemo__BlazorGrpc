using BlazorGrpc.Server;
using BlazorGrpc.Server.Infrastructure;
using BlazorGrpc.Server.Services;
using BlazorGrpc.Server.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddScoped<WeatherForecastService>();
services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));

services.AddScoped<IMongoDBContext, MongoDBContext>();

services.AddScoped<ILocationDAO, LocationDAO>();
services.AddScoped<ILogger, Logger<LocationDAO>>();
services.AddScoped<ILogger, Logger<LocationService>>();
services.AddScoped<ILocationService, LocationService>();



services.AddGrpc();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<WeatherForecastService>();
    endpoints.MapGrpcService<LocationService>();
    //endpoints.MapHub<LocationHub>("/locationHub");

    endpoints.MapRazorPages();
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
});

app.Run();
