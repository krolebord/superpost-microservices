using System.Net.Http.Json;
using UserService.Models;

namespace Common.Clients.UserService;

public class UserServiceClient : ServiceClientBase
{
    public UserServiceClient(HttpClient httpClient) : base(httpClient)
    {}
    
    public async Task<Guid> CreateUser(UserCreateDto dto)
    {
        var response = await httpClient.PostAsJsonAsync("/", dto);
        if (!response.IsSuccessStatusCode) throw new Exception("Couldn't create user");
        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    public async Task<ICollection<UserDto>> GetUserSubscriptions(Guid userId)
    {
        return (await httpClient.GetFromJsonAsync<ICollection<UserDto>>($"subscriptions/{userId}"))!;
    }

    public async Task<ICollection<UserDto>> GetUsers(IEnumerable<Guid> ids)
    {
        var query = new QueryBuilder();
        query.AddParameter("ids", ids);
        return (await httpClient.GetFromJsonAsync<ICollection<UserDto>>($"{query}"))!;
    }
}
