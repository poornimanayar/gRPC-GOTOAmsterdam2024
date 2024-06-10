// See https://aka.ms/new-console-template for more information
using gRPC.Demo;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("https://localhost:7127");
var client = new DemoService.DemoServiceClient(channel);

var response = client.Unary(new RequestMessage(){MessageValue = "Hello from client"});
    
Console.WriteLine($"Message from server - {response.MessageValue}");

Console.ReadLine();

