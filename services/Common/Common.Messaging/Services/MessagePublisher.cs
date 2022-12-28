using System.Text.Json;
using Common.Messaging.Interfaces;
using Common.Messaging.Options;
using RabbitMQ.Client;
using ExchangeType=Common.Messaging.Options.ExchangeType;

namespace Common.Messaging.Services;

public class MessagePublisher : IMessagePublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessagePublisher(IConnectionFactory factory)
    {
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }
    
    public void Publish<T>(T message, ProduceOptions options)
    {
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.ContentType = "application/json";

        _channel.ExchangeDeclare(options.ExchangeName, options.ExchangeType.GetValue());

        var body = JsonSerializer.SerializeToUtf8Bytes(message);
        _channel.BasicPublish(options.ExchangeName, options.RoutingKey, options.Mandatory, properties, body);
    }
    
    public void PublishEvent<T>(T message, string routingKey)
    {
        Publish(message, new()
        {
            Mandatory = true,
            ExchangeName = "event.exchange",
            ExchangeType = ExchangeType.Topic,
            RoutingKey = routingKey
        });
    }

    public void Dispose()
    {
        _connection.Dispose();
        _channel.Dispose();
    }
}
