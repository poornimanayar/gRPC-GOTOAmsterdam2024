using Grpc.Core;
using Readings.V1;

namespace gRPC.DeepDive.Demo.Service;

public class ReadingBiDirectionalStreamingService : Readings.V1.ReadingsBiDirectionalStreamingService.ReadingsBiDirectionalStreamingServiceBase
{
    public override async Task ProcessReadings(IAsyncStreamReader<ProcessReadingsRequest> requestStream,
        IServerStreamWriter<ProcessReadingsResponse> responseStream, ServerCallContext context)
    {
        //send a response for each request as they are read
        //complex scenarios like reading requests and sending responses simultaneously are possible

        while (await requestStream.MoveNext())
        {
            Console.WriteLine($"Message received from client -  {requestStream.Current.Id}");

            Console.WriteLine($"Sending message to client -  {requestStream.Current.Id}");

            await Task.Delay(1500);

            //place a message on the stream
            await responseStream.WriteAsync(new ProcessReadingsResponse { Message = $"Processed {requestStream.Current.Id}" });
        }
    }
}
