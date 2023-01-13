using BlazorGrpc.Client;
using BlazorGrpc.Shared;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton(services =>
{
    var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
    var url = services.GetRequiredService<NavigationManager>().BaseUri;
    var channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions { HttpClient = httpClient });

    return new WeatherForecastService.WeatherForecastServiceClient(channel);
});

builder.Services.AddSingleton(services =>
{
    var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
    var url = services.GetRequiredService<NavigationManager>().BaseUri;
    var channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions { HttpClient = httpClient });

    return new LocationProto.LocationProtoClient(channel);
});

await builder.Build().RunAsync();
