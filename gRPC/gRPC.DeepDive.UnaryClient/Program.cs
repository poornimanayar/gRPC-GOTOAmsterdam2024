using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Readings.V1;

var channel = GrpcChannel.ForAddress("http://localhost:5022");

var callInvoker = channel.Intercept(new ClientLoggingInterceptor()).Intercept(new FirstInterceptor()).Intercept(new SecondInterceptor());

var unaryClient = new Readings.V1.ReadingsService.ReadingsServiceClient(callInvoker);


//AsyncUnaryCall<T>
var asyncResponse = unaryClient.SendReadingAsync(new SendReadingRequest
{
    Id = 1,
    DeviceName = "Poorni's device",
    Temperature = 3.5,
    UpdateTime = DateTime.UtcNow.ToTimestamp()

});

var response = await asyncResponse.ResponseAsync;

Console.WriteLine(asyncResponse.GetStatus());

foreach (var header in await asyncResponse.ResponseHeadersAsync)
{
    Console.WriteLine(header.Key + ": " + header.Value);
}

if (response is not null)
{
    Console.WriteLine(response.Message);
}


Console.ReadLine();

public class ClientLoggingInterceptor : Interceptor
{
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {

        var call = continuation(request, context);

        //get access to the response and exception using a Handler
        //manually create/adjust the response based on the result of continuation
        return new AsyncUnaryCall<TResponse>(HandleResponse(call.ResponseAsync), call.ResponseHeadersAsync, call.GetStatus, call.GetTrailers, call.Dispose);
    }

    private async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> contResponse)
    {
        //
        try
        {
            var response = await contResponse;
            Console.WriteLine("Response received"); ;
            return response;
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"Call error: {ex.Status.Detail}");
            return default;
        }
    }

}

public class FirstInterceptor : Interceptor
{

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        Console.WriteLine("Logging from FirstInterceptor");
        //return a response from the result of continuation
        return continuation(request, context);
    }

}

public class SecondInterceptor : Interceptor
{
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        Console.WriteLine("Logging from SecondInterceptor");

        //return a response from the result of continuation
        return continuation(request, context);
    }

}