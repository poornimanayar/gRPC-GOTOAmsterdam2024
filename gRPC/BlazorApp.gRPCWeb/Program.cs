using BlazorApp.gRPCWeb;
using gRPC.Demo;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddGrpcClient<DemoService.DemoServiceClient>(options =>
    {
        options.Address = new Uri("https://localhost:7127");
    }).ConfigurePrimaryHttpMessageHandler(() => new GrpcWebHandler(new HttpClientHandler()));


await builder.Build().RunAsync();
