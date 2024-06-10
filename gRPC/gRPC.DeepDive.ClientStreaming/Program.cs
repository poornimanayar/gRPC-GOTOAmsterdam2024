
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("https://localhost:7139");

var clientStreamingCall = new Readings.V1.ReadingsClientStreamingService.ReadingsClientStreamingServiceClient(channel);

//open the client stream, no message sent, client invokes the method
var call = clientStreamingCall.SendReadings();

var randomDouble = new Random();

for (int i = 1; i <= 10; i++)
{
    Console.WriteLine($"Sending message - {i}");

    //place a new message on the stream, available to server at this point
    await call.RequestStream.WriteAsync(new Readings.V1.SendReadingsRequest()
    {
        Id = i,
        DeviceName = "Poornima's test device",
        Temperature = randomDouble.NextDouble(),
        UpdatedOn = DateTime.UtcNow.ToTimestamp()
    });

    await Task.Delay(1000);
}

//close the stream to notify of streaming complete
await call.RequestStream.CompleteAsync();

//await the call
var response = await call;

Console.WriteLine(call.GetStatus());

Console.WriteLine($"Message from server - number processed - {response.NumberProcessed}");

Console.ReadLine();