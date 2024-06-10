using gRPC.Demo.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

builder.Services.AddGrpcReflection();


var app = builder.Build();
app.UseCors();
app.UseGrpcWeb();
// Configure the HTTP request pipeline.
app.MapGrpcService<DemoService>().EnableGrpcWeb().RequireCors("AllowAll"); ;
app.MapGrpcReflectionService();

app.Run();
