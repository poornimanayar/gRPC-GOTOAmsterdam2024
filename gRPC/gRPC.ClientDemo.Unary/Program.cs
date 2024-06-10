using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Readings.V1;

var channel = GrpcChannel.ForAddress("https://localhost:7139");
var unaryClient = new Readings.V1.ReadingsService.ReadingsServiceClient(channel);

//synchronous blocking call
var response = unaryClient.SendReading(new SendReadingRequest
{
    Id = 1,
    DeviceName = "Poorni's device",
    Temperature = 34.5,
    UpdateTime = DateTime.UtcNow.ToTimestamp()

});

Console.WriteLine(response.Message);

//async non-blocking call, returns ASyncUnaryCall<T>
//store call in a variable

//var call = unaryClient.SendReadingAsync(new SendReadingRequest
//{
//    Id = 1,
//    DeviceName = "Poorni's device",
//    Temperature = 34.5,
//    UpdateTime = DateTime.UtcNow.ToTimestamp()

//});

//var asyncResponse = await call.ResponseAsync;

//var headers = await call.ResponseHeadersAsync;

//  Console.WriteLine(asyncResponse.Message);

//foreach (var header in headers)
//{
//    Console.WriteLine(header.Key + ": " + header.Value);
//}

Console.ReadLine();

