using Greet.V2;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("https://localhost:7237");
var client = new Greet.V2.Greeter.GreeterClient(channel);

//await the call, returns a Task
var response = client.SayHello(new HelloRequest() { Name = "client", Notes = "talking to v2!"});

Console.WriteLine($"Message from server - {response.Message}");

Console.ReadLine();