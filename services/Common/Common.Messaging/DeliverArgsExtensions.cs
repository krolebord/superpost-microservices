using System.Text.Json;
using RabbitMQ.Client.Events;

namespace Common.Messaging;

public static class DeliverArgsExtensions
{
    public static T? DeserializeBodyAsJson<T>(this BasicDeliverEventArgs message)
    {
        if (!message.BasicProperties.IsContentTypePresent() || message.BasicProperties.ContentType != "application/json")
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(message.Body.Span);
    }
}
