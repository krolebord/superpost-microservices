using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Clients.UserService;

public static class AddUserServiceClientExtension
{
    public static IServiceCollection AddUserServiceClient(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddServiceClient<UserServiceClient>(configuration.GetServiceClientOptions("UserService"));
    }
}
