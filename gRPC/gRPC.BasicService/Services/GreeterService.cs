using Grpc.Core;
using gRPC.BasicService;

namespace gRPC.BasicService.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;

    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    //ServerCallContext - access to context info
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        }) ;
    }

    public override Task<DemoMethodReply> MyDemoMethod(DemoMethodRequest request, ServerCallContext context)
    {
        return Task.FromResult(new DemoMethodReply()
        {
            Message = "Hello from demo method"
        });
    }
}