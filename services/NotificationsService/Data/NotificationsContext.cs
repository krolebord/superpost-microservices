using Microsoft.EntityFrameworkCore;
using NotificationsService.Models;

namespace NotificationsService.Data;

public class NotificationsContext : DbContext
{
    public DbSet<Notification> Notifications { get; set; } = null!;
    
    public NotificationsContext()
    {}

    public NotificationsContext(DbContextOptions<NotificationsContext> options)
        : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Notification>(x => {
            x.HasIndex(x => x.UserId);
            x.OwnsOne(x => x.Context);
        });
    }
}
