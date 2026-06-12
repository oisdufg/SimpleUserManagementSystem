using UserSystemService.Models;
using UserSystemService.Models.DTO;

namespace UserSystemService.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<UserShortInfo>> GetAllUserNamesAsync(CancellationToken cancellationToken);
    Task<User> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task CreateAsync(User user, CancellationToken cancellationToken);
    Task UpdateAsync(User user, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}