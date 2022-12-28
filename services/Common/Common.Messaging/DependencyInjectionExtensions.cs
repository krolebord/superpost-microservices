using Common.Messaging.Interfaces;
using Common.Messaging.Models;
using Common.Messaging.Options;
using Common.Messaging.Services;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Common.Messaging;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, RabbitMQOptions options)
    {
        services.AddSingleton<IConnectionFactory>(_ => new ConnectionFactory
        {
            Uri = new Uri(options.ConnectionString),
            DispatchConsumersAsync = true
        });

        services.AddSingleton<IMessageConsumer, MessageConsumer>();
        services.AddSingleton<IMessagePublisher, MessagePublisher>();
        services.AddHostedService<ConsumerHostedService>();

        return services;
    }

    public static IServiceCollection AddMessageHandler<TMessage, THandler>(this IServiceCollection services, ConsumeOptions options)
        where THandler : IMessageHandler<TMessage>
    {
        services.AddTransient(typeof(THandler));
        services.AddSingleton(new RegisteredHandlerInfo(typeof(THandler), typeof(TMessage), options));

        return services;
    }
}
