using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Readings.V1;

var channel = GrpcChannel.ForAddress("https://localhost:7139");

var biDirectionalStreamingCall = new ReadingsBiDirectionalStreamingService.ReadingsBiDirectionalStreamingServiceClient(channel);

//open the stream, no message sent, client invokes the method
using (var call = biDirectionalStreamingCall.ProcessReadings())
{
    //start listening from the server
    var responseReaderTask = Task.Run(async () =>
    {
        //MoveNext() will be true until server stops streaming
        while (await call.ResponseStream.MoveNext())
        {
            var message = call.ResponseStream.Current;
            Console.WriteLine($"Received - {message.Message}");
        }
    });

    var randomDouble = new Random();

    for (int i = 1; i <= 10; i++)
    {
        Console.WriteLine($"Sending message - {i}");

        var request = new ProcessReadingsRequest()
        {
            Id = i,
            DeviceName = "Poornima's test device",
            Temperature = randomDouble.NextDouble(),
            UpdatedOn = DateTime.UtcNow.ToTimestamp()
        };

        //place a new message on the stream, available to server at this point
        await call.RequestStream.WriteAsync(request);

        await Task.Delay(2000);
    }
    //close client stream, server notified that client has stopped streaming
    await call.RequestStream.CompleteAsync();

    //wait for the response stream 
    await responseReaderTask;
}

Console.ReadLine();