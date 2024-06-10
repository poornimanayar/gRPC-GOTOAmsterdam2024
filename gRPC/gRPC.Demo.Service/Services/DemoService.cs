using Grpc.Core;

namespace gRPC.Demo.Service.Services
{
    public class DemoService : gRPC.Demo.DemoService.DemoServiceBase
    {
        public override Task<ReplyMessage> Unary(RequestMessage request, ServerCallContext context)
        {
            Console.WriteLine("===========OUTPUT===================");
            Console.WriteLine($"Message recieved -  {request.MessageValue}");

            return Task.FromResult(new ReplyMessage() { MessageValue = $"Processed {request.MessageValue}" });
        }

        public override async Task ServerStreamDemo(RequestMessage request, 
            IServerStreamWriter<ReplyMessage> responseStream, ServerCallContext context)
        {
            Console.WriteLine("===========OUTPUT===================");
            Console.WriteLine($"Message from client - {request.MessageValue}");

            for (int i = 1; i <= 500; i++)
            {
                var message = new ReplyMessage
                {
                    MessageValue = "Hello " + i
                };

                Console.WriteLine($"Sending to client -  {message}");

                //place a message to the stream, immediately available to the client
                await responseStream.WriteAsync(message);
            }
        }


        public override async Task<ReplyMessage> ClientStreamDemo(IAsyncStreamReader<RequestMessage> requestStream, 
            ServerCallContext context)
        {
            int messageCount = 0;

            Console.WriteLine("===========OUTPUT===================");
            //advance the stream reader to the next element, returns false if end of stream has reached
            while (await requestStream.MoveNext())
            {
                Console.WriteLine($"Message received from client -  {requestStream.Current}");
                messageCount++;
            }

            return new ReplyMessage { MessageValue = $"Processed {messageCount} messages" };
        }


        public override async Task BidirectionalStreamDemo(IAsyncStreamReader<RequestMessage> requestStream, 
            IServerStreamWriter<ReplyMessage> responseStream, ServerCallContext context)
        {
            Console.WriteLine("===========OUTPUT===================");
            //send a response for each request as they are read
            //complex scenarios like reading requests and sending responses simultaneously are possible
            while (await requestStream.MoveNext())
            {
                Console.WriteLine($"Message received from client -  {requestStream.Current}");

                Console.WriteLine($"Sending message to client -  {requestStream.Current}");

                //place a message on the stream
                await responseStream.WriteAsync(new ReplyMessage(){MessageValue = $"Processed {requestStream.Current}"});

            }   
        }



        public override Task<ReplyMessage> HeaderAndTrailerUnaryDemo(RequestMessage request, ServerCallContext context)
        {
            Console.WriteLine("===========OUTPUT===================");
            Console.WriteLine($"Message recieved from client -  {request.MessageValue}");

            //access request headers
            foreach (var header in context.RequestHeaders)
            {
               Console.WriteLine($"{header.Key} - {header.Value}");
            }

            var metaData = new Metadata { { "my-response-header", "my-response-header-value" } };

            //add response headers
            context.WriteResponseHeadersAsync(metaData);

            //add response trailer
            context.ResponseTrailers.Add("my-response-trailer", $"{context.RequestHeaders.Count} headers processed");

            return Task.FromResult(new ReplyMessage() { MessageValue = $"Processed {request.MessageValue}" });
        }
    }
}
