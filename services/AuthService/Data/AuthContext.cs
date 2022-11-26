using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data;

public class AuthContext : DbContext
{
    public DbSet<Credentials> Credentials { get; set; } = null!;
    
    public AuthContext()
    {}

    public AuthContext(DbContextOptions<AuthContext> options)
        : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Credentials>(x => {
            x.HasIndex(credentials => credentials.UserName).IsUnique();
            x.HasIndex(credentials => credentials.Email).IsUnique();
            x.HasIndex(credentials => credentials.UserId).IsUnique();
        });
    }
}
