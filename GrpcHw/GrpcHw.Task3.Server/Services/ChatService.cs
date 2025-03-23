using System.Collections.Concurrent;
using ChatServer;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace GrpcHw.Task3.Server.Services
{
    [Authorize]
    public class ChatService : ChatServer.ChatService.ChatServiceBase
    {
        private static readonly ConcurrentQueue<ChatMessage> Messages = new();
        private static readonly List<IServerStreamWriter<ChatMessage>> Subscribers = new();

        public override async Task<Empty> SendMessage(MessageRequest request, ServerCallContext context)
        {
            var message = new ChatMessage { User = request.User, Text = request.Text };
            Messages.Enqueue(message);

            lock (Subscribers)
            {
                foreach (var subscriber in Subscribers)
                {
                    subscriber.WriteAsync(message);
                }
            }

            return new Empty();
        }

        public override async Task ReceiveMessages(Empty request, IServerStreamWriter<ChatMessage> responseStream, ServerCallContext context)
        {
            lock (Subscribers)
            {
                Subscribers.Add(responseStream);
            }

            try
            {
                while (!context.CancellationToken.IsCancellationRequested)
                {
                    await responseStream.WriteAsync(new ChatMessage
                    {
                        User = "test server",
                        Text = "asdasdad"
                    });
                    
                    
                    await Task.Delay(1000);
                }
            }
            finally
            {
                lock (Subscribers)
                {
                    Subscribers.Remove(responseStream);
                }
            }
        }
    }
}