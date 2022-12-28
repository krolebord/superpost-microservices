namespace PostService.Models;

public class PostCreatedEto
{
    public required Guid PostId { get; set; }
    public required Guid AuthorId { get; set; }
    public required DateTime CreatedAt { get; set; }
}
