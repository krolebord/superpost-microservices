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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Post>(postBuilder => {
            postBuilder.HasIndex(x => x.UserId);
            postBuilder
                .HasOne(x => x.ParentPost)
                .WithMany(x => x.SubPosts)
                .HasForeignKey(x => x.ParentPostId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}
