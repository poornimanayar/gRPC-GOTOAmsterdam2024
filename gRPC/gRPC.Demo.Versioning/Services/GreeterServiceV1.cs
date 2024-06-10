using Greet.V1;
using Grpc.Core;

namespace gRPC.Demo.Versioning.Services;

public class GreeterServiceV1 : Greet.V1.Greeter.GreeterBase
{
    private readonly ILogger<GreeterServiceV1> _logger;

    public GreeterServiceV1(ILogger<GreeterServiceV1> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        Console.WriteLine(request.Name);
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }
}