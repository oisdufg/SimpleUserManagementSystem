using Microsoft.EntityFrameworkCore;
using UserSystemService.Configurations;
using UserSystemService.Interfaces;
using UserSystemService.Models;

namespace UserSystemService.Context;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserTask> UserTasks { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserTaskConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}