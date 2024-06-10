// See https://aka.ms/new-console-template for more information

using gRPC.Demo;
using Grpc.Core;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("https://localhost:7127");
var client = new DemoService.DemoServiceClient(channel);

//trigger the server stream by sending a message
var call = client.ServerStreamDemo(new RequestMessage(){MessageValue = "Hello from client"});

//read from the stream till there are no more messages to read 
await foreach (var response in call.ResponseStream.ReadAllAsync())
{
    Console.WriteLine($"{response.MessageValue}");
}

Console.ReadLine();
