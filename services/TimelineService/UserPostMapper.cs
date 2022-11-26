using PostService.Models;
using TimelineService.Models;

namespace TimelineService;

public static class UserPostMapper
{
    public static UserPostDto CreateUserPostDto(PostDto post, UserService.Models.UserDto user) => new()
    {
        Id = post.Id,
        Content = post.Content,
        CreatedAt = post.CreatedAt,
        SubPostsCount = post.SubPostsCount,
        User = user.ToUserDto()
    };

    public static UserDto ToUserDto(this UserService.Models.UserDto user) => new()
    {
        Id = user.Id,
        Name = user.Name
    };
}
