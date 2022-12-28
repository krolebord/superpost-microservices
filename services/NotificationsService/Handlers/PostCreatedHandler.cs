using Common.Clients.UserService;
using Common.Messaging.Interfaces;
using Common.Messaging.Models;
using NotificationsService.Data;
using NotificationsService.Models;
using NotifierService.Models;
using PostService.Models;
using UserService.Models;

namespace NotificationsService.Handlers;

public class PostCreatedHandler : IMessageHandler<PostCreatedEto>
{
    private readonly UserServiceClient _userServiceClient;
    private readonly NotificationsContext _context;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<PostCreatedHandler> _logger;

    public PostCreatedHandler(
        UserServiceClient userServiceClient,
        NotificationsContext context,
        IMessagePublisher publisher,
        ILogger<PostCreatedHandler> logger)
    {
        _userServiceClient = userServiceClient;
        _context = context;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(MessageDeliverArgs<PostCreatedEto> args)
    {
        if (args.Message is null)
        {
            args.Ack(true);
            return;
        }
        
        var subscribers = await GetSubscribers(args.Message.AuthorId);
        
        if (subscribers is null)
        {
            args.Ack(true);
            return;
        }

        var notifications = subscribers.Select(subscriber => new Notification
        {
            Id = Guid.NewGuid(),
            UserId = subscriber.Id,
            CreatedAt = args.Message.CreatedAt,
            Message = $"New post by {subscriber.Name}",
            Context = new()
            {
                Id = args.Message.PostId.ToString(),
                Type = "post-create"
            }
        }).ToList();
        _context.Notifications.AddRange(notifications);
        await _context.SaveChangesAsync();

        args.Ack(true);
        
        foreach (var notification in notifications)
        {
            _publisher.PublishEvent<NotificationEto>(new()
            {
                UserId = notification.UserId,
                Notification = NotificationMappers.NotificationDto(notification)
            }, $"event.notification.create.{notification.Id}");
        }
    }

    public async Task<ICollection<UserDto>?> GetSubscribers(Guid userId)
    {
        try
        {
            return await _userServiceClient.GetSubscribers(userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Couldn't fetch subscribers while post created event");
            return null;
        }
    }
}
