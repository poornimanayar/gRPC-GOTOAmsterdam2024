// See https://aka.ms/new-console-template for more information
using Grpc.Core;
using gRPC.Demo;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress("https://localhost:7127");
var client = new DemoService.DemoServiceClient(channel);


using (var call = client.BidirectionalStreamDemo())
{
    //start listening from the server
    var responseReaderTask = Task.Run(async () =>
    {
        while (await call.ResponseStream.MoveNext())
        {
            var message = call.ResponseStream.Current;
            Console.WriteLine("Received " + message.MessageValue);
        }
    });
    for (int i = 1; i <= 100; i++)
    {
        var request = new RequestMessage
        {
            MessageValue = $"Hello from {i}"
        };
        Console.WriteLine("Sending " + request.MessageValue);

        //start the client stream
        await call.RequestStream.WriteAsync(request);
    }
    //close client stream
    await call.RequestStream.CompleteAsync();
    //wait for the response stream 
    await responseReaderTask;
}

Console.ReadLine();