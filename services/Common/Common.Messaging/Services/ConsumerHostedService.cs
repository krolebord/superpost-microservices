using Common.Messaging.Interfaces;
using Common.Messaging.Models;
using RabbitMQ.Client.Events;

namespace Common.Messaging.Services;

public class ConsumerHostedService : IHostedService
{
    private readonly IEnumerable<RegisteredHandlerInfo> _handlerInfos;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessageConsumer _consumer;
    private readonly ILogger<ConsumerHostedService> _logger;

    private readonly List<Action?> _unsubscribeActions = new();

    public ConsumerHostedService(IEnumerable<RegisteredHandlerInfo> handlerInfos, IServiceProvider serviceProvider, IMessageConsumer consumer, ILogger<ConsumerHostedService> logger)
    {
        _handlerInfos = handlerInfos;
        _serviceProvider = serviceProvider;
        _consumer = consumer;
        _logger = logger;

    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var handlerInfo in _handlerInfos)
        {
            AsyncEventHandler<MessageDeliverArgs> handler = async (_, args) => {
                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService(handlerInfo.HandlerType);

                var handleMethod = handler.GetType().GetMethod(nameof(IMessageHandler<object>.Handle));
                await (Task) handleMethod?.Invoke(handler, new object?[]
                {
                    args
                })!;
                _logger.LogInformation("Successfully processed message of type: {Type}", handlerInfo.MessageType.Name);
            };

            var method = typeof(IMessageConsumer)
                .GetMethod(nameof(IMessageConsumer.Subscribe))!
                .MakeGenericMethod(handlerInfo.MessageType);

            var unsubscribe = method.Invoke(_consumer, new object?[] { handler, handlerInfo.Options });
            _unsubscribeActions.Add((Action) unsubscribe!);
            
            _logger.LogInformation("Registered message handler of type: {Type}", handlerInfo.MessageType.Name);
        }

        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var unsubscribeAction in _unsubscribeActions)
        {
            unsubscribeAction?.Invoke();
        }
        _unsubscribeActions.Clear();
        
        return Task.CompletedTask;
    }
}
