using gRPC.DeepDive.Demo.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

app.MapGrpcService<ReadingService>();
app.MapGrpcService<ReadingServerStreamingService>();
app.MapGrpcService<ReadingClientStreamingService>();
app.MapGrpcService<ReadingBiDirectionalStreamingService>();
app.MapGrpcReflectionService();
app.Run();
