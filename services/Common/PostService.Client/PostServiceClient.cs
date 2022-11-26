using System.Net.Http.Json;
using PostService.Models;

namespace Common.Clients.PostService;

public class PostServiceClient : ServiceClientBase
{
    public PostServiceClient(HttpClient httpClient) : base(httpClient)
    {}

    public async Task<FullPostDto?> GetFullPost(Guid postId)
    {
        return await httpClient.GetFromJsonAsync<FullPostDto>($"/{postId}");
    }

    public async Task<ICollection<PostDto>> GetLastPosts(
        int? limit = null,
        bool? filterSubPosts = null,
        Guid? fromUser = null,
        IEnumerable<Guid>? fromUsers = null)
    {
        if (fromUser is not null)
        {
            fromUsers ??= new[] { fromUser.Value };
        }
        
        var query = new QueryBuilder();
        if (limit is not null)
        {
            query.AddParameter("limit", limit.Value);
        }
        if (filterSubPosts is not null)
        {
            query.AddParameter("filterSubPosts", filterSubPosts);
        }
        if (fromUsers is not null)
        {
            query.AddParameter("fromUser", fromUsers);
        }
        return await httpClient.GetFromJsonAsync<ICollection<PostDto>>($"/last{query}") ?? throw new Exception();
    }
}
