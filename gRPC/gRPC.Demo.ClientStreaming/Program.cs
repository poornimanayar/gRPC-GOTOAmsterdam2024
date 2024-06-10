// See https://aka.ms/new-console-template for more information

using gRPC.Demo;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("https://localhost:7127");

var client = new DemoService.DemoServiceClient(channel);

//create an instance of AsyncClientStreamingCall
var call = client.ClientStreamDemo();

for (int i = 1; i <= 100; i++)
{
    //place a new message on the stream, available to server at this point
    await call.RequestStream.WriteAsync(new RequestMessage() { MessageValue = $"Hello from {i}" });
}

//close the stream to notify of streaming complete
await call.RequestStream.CompleteAsync();

//await the call
var response = await call;

Console.WriteLine($"Message from server - {response.MessageValue}");


Console.ReadLine();