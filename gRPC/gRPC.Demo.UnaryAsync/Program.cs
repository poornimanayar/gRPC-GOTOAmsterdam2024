using gRPC.Demo;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("https://localhost:7127");
var client = new DemoService.DemoServiceClient(channel);

//store call in a variable, returns a AsyncUnaryCall
var unaryCall = client.UnaryAsync(new RequestMessage() { MessageValue = "Hello from client" });

//await and receive the response 
var response = await unaryCall.ResponseAsync;

//await and receive the response headers
var headers = await unaryCall.ResponseHeadersAsync;

Console.WriteLine($"Message from server - {response.MessageValue}");

Console.ReadLine();