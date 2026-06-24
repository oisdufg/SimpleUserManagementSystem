using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UserSystemService.Models;

namespace UserSystemService.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<UserTask> UserTasks { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    int SaveChanges();
    
    EntityEntry Entry(object entity);
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}