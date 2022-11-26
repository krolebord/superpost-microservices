using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Clients.PostService;

public static class AddUserServiceClientExtension
{
    public static IServiceCollection AddPostServiceClient(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddServiceClient<PostServiceClient>(configuration.GetServiceClientOptions("PostService"));
    }
}
