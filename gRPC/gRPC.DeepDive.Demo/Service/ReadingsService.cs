using Grpc.Core;
using Readings.V1;

namespace gRPC.DeepDive.Demo.Service;

public class ReadingService : Readings.V1.ReadingsService.ReadingsServiceBase
{
    public override Task<SendReadingResponse> SendReading(SendReadingRequest request, ServerCallContext context)
    {
        return Task.FromResult(new SendReadingResponse()
        {
            Message = $"Reading recorded successfully for :{request.DeviceName}"
        });
    }
}