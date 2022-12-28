using Common.Messaging.Models;

namespace Common.Messaging.Interfaces;

public interface IMessageHandler<TMessage>
{
    Task Handle(MessageDeliverArgs<TMessage> args);
}
