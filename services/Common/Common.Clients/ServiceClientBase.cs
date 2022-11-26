using System.Net.Http;

namespace Common.Clients;

public abstract class ServiceClientBase
{
    protected readonly HttpClient httpClient;

    public HttpClient HttpClient => httpClient;

    protected ServiceClientBase(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
}
