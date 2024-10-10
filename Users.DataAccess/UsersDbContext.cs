using Microsoft.EntityFrameworkCore;
using Users.Domain.Models;

namespace Users.DataAccess;

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options) { }
    
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>().HasKey(u => u.Id);
        
        modelBuilder.Entity<User>().Property(u => u.UserName)
            .HasMaxLength(User.MaxUserNameLength)
            .IsRequired();

        modelBuilder.Entity<User>().Property(u => u.PasswordHash)
            .IsRequired();

        modelBuilder.Entity<User>().Property(u => u.Email)
            .IsRequired();
    }
}