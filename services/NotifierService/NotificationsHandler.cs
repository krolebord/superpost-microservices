using System.Text.Json;
using Common.Messaging.Interfaces;
using Common.Messaging.Models;
using NotifierService.Models;

namespace NotifierService;

public class NotificationsHandler : IMessageHandler<NotificationEto>
{
    private readonly INotifierService _notifierService;

    public NotificationsHandler(INotifierService notifierService)
    {
        _notifierService = notifierService;
    }
    
    public async Task Handle(MessageDeliverArgs<NotificationEto> args)
    {
        if (args.Message is null)
        {
            args.Ack(true);
            return;
        }

        var serializedNotification = JsonSerializer.Serialize(args.Message.Notification);
        await _notifierService.SendEventAsync(serializedNotification, client => client.Id == args.Message.UserId);
        args.Ack(true);
    }
}
