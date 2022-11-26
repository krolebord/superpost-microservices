namespace PostService.Models;

public class Post
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }

    public required string Content { get; set; }
    
    public required DateTime CreatedAt { get; set; }
    
    public Guid? ParentPostId { get; set; }
    public Post? ParentPost { get; set; }

    public ICollection<Post> SubPosts { get; } = new HashSet<Post>();
}
