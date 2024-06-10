using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("https://localhost:7139");
try
{
    var streamingClient = new Readings.V1.ReadingsServerStreamingService.ReadingsServerStreamingServiceClient(channel);
    
    var callMetaData = new Metadata
    {
        new Metadata.Entry ("my-key", "my-value" )
    };

    //AsyncServerStreamingCall<T>
    // deadline added via ChannelOptions
    var streamingCall = streamingClient.GetReadings(new Readings.V1.GetReadingsRequest
    { UpdatedOn = DateTime.UtcNow.AddDays(-1).ToTimestamp() }, callMetaData, deadline: DateTime.UtcNow.AddMinutes(1));


    var responseHeaders = await streamingCall.ResponseHeadersAsync;

    foreach (var responseHeader in responseHeaders)
    {
        Console.WriteLine($"Header key = {responseHeader.Key} Header Value = {responseHeader.Value}");
    }


    //Read all the messages from the response stream
    await foreach (var reading in streamingCall.ResponseStream.ReadAllAsync())
    {
        Console.WriteLine($"{reading.Id} - {reading.DeviceName} - {reading.Temperature} -{reading.UpdatedOn.ToDateTime()}");
    }

    var responseTrailers = streamingCall.GetTrailers();

    foreach (var responseTrailer in responseTrailers)
    {
        Console.WriteLine($"Trailer key = {responseTrailer.Key} Trailer Value = {responseTrailer.Value}");
    }


    Console.WriteLine(streamingCall.GetStatus());


}

catch (RpcException ex) when (ex.StatusCode == StatusCode.DeadlineExceeded)
{
    Console.WriteLine("Stream cancelled/timeout.");

    var responseTrailers = ex.Trailers;

    foreach (var responseTrailer in responseTrailers)
    {
        Console.WriteLine($"Trailer key = {responseTrailer.Key} Trailer Value = {responseTrailer.Value}");
    }
}

Console.ReadLine();


public class ServerStreamingInterceptor : Interceptor
{
    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        
            var call = continuation(request, context);

            return new AsyncServerStreamingCall<TResponse>(
                 HandleResponse(call.ResponseStream),
                 call.ResponseHeadersAsync,
                 call.GetStatus,
                 call.GetTrailers,
                 call.Dispose);
      
    }

    private IAsyncStreamReader<TResponse> HandleResponse<TResponse>(IAsyncStreamReader<TResponse> stream)
    {
        try
        {
            Console.WriteLine("here");
            return stream;
            
        }
        catch (RpcException ex)
        {
            Console.WriteLine($"{ex.Message}");
            throw ex;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Custom error", ex);
        }
    }
}