using UserSystemService.Context;
using UserSystemService.Interfaces;
using UserSystemService.Models;
using Microsoft.EntityFrameworkCore;
using UserSystemService.Models.DTO;

namespace UserSystemService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IApplicationDbContext _context;
    
    public UserRepository(IApplicationDbContext context)
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

    public async Task<IEnumerable<UserShortInfoRequest>> GetAllUserNamesAsync(CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .OrderBy(e => e.LastName)
            .Select(e => new UserShortInfoRequest
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
        
        User entity = await _context.Users.FirstAsync(e => e.ID == user.ID, cancellationToken);
        
        entity.FirstName = user.FirstName;
        entity.MiddleName = user.MiddleName;
        entity.LastName = user.LastName;
        entity.Email = user.Email;
        entity.Birthday = user.Birthday; 
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await CheckIfUserExists(id, cancellationToken);
        
        User user = await _context.Users.FindAsync(id, cancellationToken);

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    private async Task CheckIfUserExists(int id, CancellationToken cancellationToken)
    {
        bool data = await _context.Users.AsNoTracking().AnyAsync(e => e.ID == id, cancellationToken);
        if (data is false)
        {
            throw new KeyNotFoundException($"User with id {id} not found");
        }
    }
}