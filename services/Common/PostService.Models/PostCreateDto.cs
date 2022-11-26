namespace PostService.Models;

public record PostCreateDto(string Content, Guid? ParentPostId);
