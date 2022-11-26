using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data;

public class UsersContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<UserSubscription> Subscriptions { get; set; } = null!;

    public UsersContext()
    {}

    public UsersContext(DbContextOptions<UsersContext> options)
        : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(userBuilder => {
            userBuilder
                .HasMany(x => x.Subscribers)
                .WithMany(x => x.SubscribedTo)
                .UsingEntity<UserSubscription>(
                    rightBuilder => rightBuilder
                        .HasOne(x => x.User)
                        .WithMany()
                        .HasForeignKey(x => x.UserId)
                        .OnDelete(DeleteBehavior.Cascade),
                    leftBuilder => leftBuilder
                        .HasOne(x => x.SubscribedTo)
                        .WithMany()
                        .HasForeignKey(x => x.SubscribedToId)
                        .OnDelete(DeleteBehavior.Cascade),
                    subscriptionBuilder => subscriptionBuilder
                        .HasKey(x => new { x.UserId, x.SubscribedToId }));
        });
    }
}
