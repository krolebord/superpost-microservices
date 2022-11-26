using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Clients;

public record ServiceClientOptions(string Url);

public static class AddServiceClientExtensions
{
    public static ServiceClientOptions GetServiceClientOptions(this IConfiguration configuration, string sectionName)
    {
        return configuration.GetSection(sectionName).Get<ServiceClientOptions>() ?? throw new Exception($"${sectionName} is not specified");
    }
    
    public static IServiceCollection AddServiceClient<T>(this IServiceCollection services, ServiceClientOptions options)
        where T : ServiceClientBase
    {
        services.AddHttpContextAccessor();
        services.AddHttpClient<T>((provider, client) => {
            client.BaseAddress = new Uri(options.Url);

            var context = provider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            if (context is not null && context.Request.Headers.Cookie.Any())
            {
                client.DefaultRequestHeaders.Add("Cookie", context.Request.Headers.Cookie.ToString());
            }
        });

        return services;
    }
}
