using Common.Auth;
using Lib.AspNetCore.ServerSentEvents;

namespace NotifierService;

public class ServerSentEventsClientIdProvider : IServerSentEventsClientIdProvider
{
    public Guid AcquireClientId(HttpContext context)
    {
        return context.User.GetUserId();
    }

    public void ReleaseClientId(Guid clientId, HttpContext context)
    {}
}
