using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.Messaging.Models;

public class MessageDeliverArgs : BasicDeliverEventArgs
{
    public Action<bool> Ack { get; }

    public MessageDeliverArgs(BasicDeliverEventArgs args, Action<bool> ack)
        : base(args.ConsumerTag, args.DeliveryTag, args.Redelivered, args.Exchange, args.RoutingKey, args.BasicProperties, args.Body)
    {
        Ack = ack;
    }
}

public class MessageDeliverArgs<TMessage> : MessageDeliverArgs
{
    private Lazy<TMessage?> _message;
    public TMessage? Message => _message.Value;

    public MessageDeliverArgs(BasicDeliverEventArgs args, Action<bool> ack) : base(args, ack)
    {
        _message = new(this.DeserializeBodyAsJson<TMessage>);
    }
}
