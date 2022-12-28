using Common.Messaging.Options;

namespace Common.Messaging.Interfaces;

public interface IMessagePublisher
{
    void Publish<T>(T message, ProduceOptions options);

    void PublishEvent<T>(T message, string routingKey);
}
