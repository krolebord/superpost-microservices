using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data;

public class UsersContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    
    public UsersContext()
    {}

    public UsersContext(DbContextOptions<UsersContext> options)
        : base(options) {}
}
