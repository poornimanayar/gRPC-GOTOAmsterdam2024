using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Readings.V1;

namespace gRPC.DeepDive.Demo.Service;

public class ReadingServerStreamingService : Readings.V1.ReadingsServerStreamingService.ReadingsServerStreamingServiceBase
{
    public override async Task GetReadings(GetReadingsRequest request, IServerStreamWriter<GetReadingsResponse> responseStream, ServerCallContext context)
    {
        var randomDouble = new Random();
        int i = 1;

        var clientHeader = context.RequestHeaders.GetValue("my-key");

        //response sent when this method is called or when first message is written to the response stream
        await context.WriteResponseHeadersAsync(new Metadata()
        {
            new Metadata.Entry  ("my-server-header", $"yes-{clientHeader}")
        });

     
            while (!context.CancellationToken.IsCancellationRequested && i <= 10)
            {
                var reading = new GetReadingsResponse
                {
                    Id = i,
                    DeviceName = $"Poorni's device - {i}",
                    Temperature = randomDouble.NextDouble(),
                    UpdatedOn = DateTime.UtcNow.ToTimestamp(),
                };

                Console.WriteLine($"Sending readings for {request.UpdatedOn.ToDateTime():dd MMMM yyyy} to client -  {reading}");
                
                //place a message to the stream, immediately available to the client
                await responseStream.WriteAsync(reading);
                        
                await Task.Delay(TimeSpan.FromSeconds(1));

                i++;
            }
        
              context.ResponseTrailers.Add("message-count", (i-1).ToString());

        if (context.CancellationToken.IsCancellationRequested)
        {
            throw new RpcException(new Status(StatusCode.DeadlineExceeded, "Deadline exceeded"), new Metadata() { new Metadata.Entry ( "processed", "no") });
        }
    } 
}

