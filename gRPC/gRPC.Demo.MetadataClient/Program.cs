using Grpc.Core;
using gRPC.Demo;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("https://localhost:7127");
var client = new DemoService.DemoServiceClient(channel);

//create a Metadata object and add metadata entries (key/value pairs)
var metaData = new Metadata();

for (int i = 0; i < 5; i++)
{
    metaData.Add($"my-request-header-{i}",$"my-request-header-{i}-value");
}

//store call in a variable, returns a AsyncUnaryCall
var unaryCall = client.HeaderAndTrailerUnaryDemoAsync
    (new RequestMessage() { MessageValue = "Hello from client" }, metaData);


//await and receive the response 
var response = await unaryCall.ResponseAsync;

//await and receive the response headers
var headers = await unaryCall.ResponseHeadersAsync;

//get the trailers after awaiting response for unary & client streaming
//get the trailers after awaiting the response stream for server and bi-directional streaming
var trailers = unaryCall.GetTrailers();

Console.WriteLine($"Message from server - {response.MessageValue}");

foreach (var header in headers)
{
    Console.WriteLine($"{header.Key} - {header.Value}");
}
foreach (var trailer in trailers)
{
    Console.WriteLine($"{trailer.Key} - {trailer.Value}");
}

Console.ReadLine();