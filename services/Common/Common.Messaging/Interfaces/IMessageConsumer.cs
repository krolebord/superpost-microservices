using Common.Messaging.Models;
using Common.Messaging.Options;
using RabbitMQ.Client.Events;

namespace Common.Messaging.Interfaces;

public interface IMessageConsumer : IDisposable
{
    Action Subscribe<TMessage>(AsyncEventHandler<MessageDeliverArgs<TMessage>> messageReceivedHandler, ConsumeOptions options);
    void SetAcknowledge(ulong deliveryTag, bool processed);
}
