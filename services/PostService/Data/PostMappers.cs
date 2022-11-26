using PostService.Models;

namespace PostService.Data;

public static class PostMappers
{
    public static Post ToPost (this PostCreateDto dto, Guid id, Guid userId, DateTime createdAt) => new()
    {
        Id = id,
        UserId = userId,
        Content = dto.Content,
        CreatedAt = createdAt,
        ParentPostId = dto.ParentPostId
    };
}
