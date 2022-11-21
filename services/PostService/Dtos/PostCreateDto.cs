using PostService.Models;

namespace PostService.Dtos;

public record PostCreateDto(string Title, string Content)
{
    public Post ToPost() => new()
    {
        Id = Guid.NewGuid(),
        Title = Title,
        Content = Content
    };
}
