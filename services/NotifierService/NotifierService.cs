using Lib.AspNetCore.ServerSentEvents;
using Microsoft.Extensions.Options;

namespace NotifierService;

public class NotifierService : ServerSentEventsService, INotifierService
{
    public NotifierService(IOptions<ServerSentEventsServiceOptions<ServerSentEventsService>> options) : base(options.ToBaseServerSentEventsServiceOptions())
    {
    }
}
