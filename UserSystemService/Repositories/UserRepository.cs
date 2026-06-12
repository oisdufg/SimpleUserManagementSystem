using UserSystemService.Context;
using UserSystemService.Interfaces;
using UserSystemService.Models;
using Microsoft.EntityFrameworkCore;
using UserSystemService.Models.DTO;

namespace UserSystemService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .OrderBy(e => e.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserShortInfo>> GetAllUserNamesAsync(CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .OrderBy(e => e.LastName)
            .Select(e => new UserShortInfo
            {
                ID = e.ID,
                FullName = $"{e.FirstName} {e.LastName} {e.MiddleName}",
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<User> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Users
                   .AsNoTracking()
                   .Include(e => e.Tasks)
                   .FirstOrDefaultAsync(e => e.ID == id, cancellationToken) ??
               throw new KeyNotFoundException($"User with {id} not found");
    }

    public async Task CreateAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        await CheckIfUserExists(user.ID, cancellationToken);
        
        await _context.Users
            .Where(e => e.ID == user.ID)
            .ExecuteUpdateAsync(e => e
                .SetProperty(p => p.FirstName, user.FirstName)
                .SetProperty(p => p.LastName, user.LastName)
                .SetProperty(p => p.MiddleName, user.MiddleName)
                .SetProperty(p => p.Email, user.Email)
                .SetProperty(p => p.Birthday, user.Birthday)
                , cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await CheckIfUserExists(id, cancellationToken);
        
        await _context.Users
            .Where(e => e.ID == id)
            .ExecuteDeleteAsync(cancellationToken);
    }
    
    private async Task CheckIfUserExists(int id, CancellationToken cancellationToken)
    {
        bool data = await _context.Users.AsNoTracking().AnyAsync(e => e.ID == id, cancellationToken);
        if (data is false)
        {
            throw new KeyNotFoundException($"User with {id} not found");
        }
    }
}