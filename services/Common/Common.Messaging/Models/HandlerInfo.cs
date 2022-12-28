using Common.Messaging.Options;
using RabbitMQ.Client.Events;

namespace Common.Messaging.Models;

public record HandlerInfo(Type MessageType, AsyncEventHandler<MessageDeliverArgs> Handler);

public record RegisteredHandlerInfo(Type HandlerType, Type MessageType, ConsumeOptions Options);
