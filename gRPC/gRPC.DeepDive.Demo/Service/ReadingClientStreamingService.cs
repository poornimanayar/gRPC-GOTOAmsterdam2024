using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Readings.V1;

namespace gRPC.DeepDive.Demo.Service;

public class ReadingClientStreamingService: Readings.V1.ReadingsClientStreamingService.ReadingsClientStreamingServiceBase
{
   public override async Task<SendReadingsResponse> SendReadings(IAsyncStreamReader<SendReadingsRequest> requestStream, ServerCallContext context)
    {
        int messageCount = 0;
               

        Console.WriteLine("===========OUTPUT===================");
        //advance the stream reader to the next element, returns false if end of stream has reached
        while (await requestStream.MoveNext())
        {
            Console.WriteLine($"Message received from client -  {requestStream.Current}");
            messageCount++;
        }
        Console.WriteLine("====================================");

        await Task.Delay(1000);       

        return new SendReadingsResponse { NumberProcessed = messageCount,UpdatedOn = DateTime.UtcNow.ToTimestamp()  };
    }
}
