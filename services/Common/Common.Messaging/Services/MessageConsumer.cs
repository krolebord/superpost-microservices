using System.Collections.Concurrent;
using System.Reflection;
using Common.Messaging.Interfaces;
using Common.Messaging.Models;
using Common.Messaging.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.Messaging.Services;

public class MessageConsumer : IMessageConsumer
{
    private readonly ILogger<MessageConsumer> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    private bool _disposed = false;

    private readonly ConcurrentDictionary<string, HandlerInfo> _handlers = new();

    public MessageConsumer(IConnectionFactory factory, ILogger<MessageConsumer> logger)
    {
        _logger = logger;
        
        _logger.LogInformation("Creating consumer mq connection");
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _logger.LogInformation("Consumer mq connected");
    }
    
    private Task ConsumerOnReceived(object sender, BasicDeliverEventArgs args)
    {
        if (!_handlers.TryGetValue(args.ConsumerTag, out var handler))
        {
            return Task.CompletedTask;
        }
        
        var ack = (bool processed) => SetAcknowledge(args.DeliveryTag, processed);

        var messageType = typeof(MessageDeliverArgs<>).MakeGenericType(handler.MessageType);
        var messageArgs = (MessageDeliverArgs)Activator.CreateInstance(messageType, args: new object[] { args, ack })!;

        return handler.Handler(sender, messageArgs);
    }


    public Action Subscribe<TMessage>(AsyncEventHandler<MessageDeliverArgs<TMessage>> messageReceivedHandler, ConsumeOptions options)
    {
        _channel.ExchangeDeclare(options.ExchangeName, options.ExchangeType.GetValue());
        _channel.QueueDeclare(options.QueueName, false, false, false);
        _channel.QueueBind(options.QueueName, options.ExchangeName, options.RoutingKey);

        if (options.SequentialFetch)
        {
            _channel.BasicQos(0, 1, false);
        }

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += ConsumerOnReceived;
        
        var tag = Guid.NewGuid().ToString();
        _handlers.TryAdd(tag, new(typeof(TMessage), (AsyncEventHandler<MessageDeliverArgs>) messageReceivedHandler));
        _channel.BasicConsume(options.QueueName, options.AutoAcknowledge, tag, consumer);
        
        return () => Unsubscribe(tag);
    }

    private void Unsubscribe(string tag)
    {
        _handlers.TryRemove(tag, out _);
        if (!_disposed)
        {
            _channel.BasicCancel(tag);
        }
    }
    
    public void SetAcknowledge(ulong deliveryTag, bool processed)
    {
        if (processed)
        {
            _channel.BasicAck(deliveryTag, false);
        }
        else
        {
            _channel.BasicNack(deliveryTag, false, true);
        }
    }
    
    public void Dispose()
    {
        _disposed = true;
        
        _connection.Close();
        _connection.Dispose();
        _channel.Close();
        _channel.Dispose();
    }
}
