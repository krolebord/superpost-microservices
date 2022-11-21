using Microsoft.EntityFrameworkCore;
using PostService.Models;

namespace PostService.Data;

public class PostsContext : DbContext
{
    public DbSet<Post> Posts { get; set; } = null!;
    
    public PostsContext()
    {}

    public PostsContext(DbContextOptions<PostsContext> options)
        : base(options) {}
}
