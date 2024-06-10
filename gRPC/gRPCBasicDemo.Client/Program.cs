// The port number must match the port of the gRPC server.

using gRPC.BasicService;
using Grpc.Net.Client;

//gRPC channel - represents a connection to the remote server. 
//Very expensive operation to create a channel, so must be reused.
using var channel = GrpcChannel.ForAddress("https://localhost:7229");
var client = new Greeter.GreeterClient(channel);

//invoke the stub method, like a local method call
var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });


Console.WriteLine("===========OUTPUT===================");
Console.WriteLine("Greeting: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();