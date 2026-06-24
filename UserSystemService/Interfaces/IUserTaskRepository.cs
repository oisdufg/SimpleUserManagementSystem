using UserSystemService.Models;
using TaskStatus = UserSystemService.Enums.TaskStatus;

namespace UserSystemService.Interfaces;

public interface IUserTaskRepository
{
    Task<IEnumerable<UserTask>> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
    Task<IEnumerable<UserTask>> GetAllAsync(CancellationToken cancellationToken);
    Task<UserTask> GetByIdAsync(int taskId, CancellationToken cancellationToken);
    Task<IEnumerable<UserTask>> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<IEnumerable<UserTask>> GetDeletedAsync(CancellationToken cancellationToken);
    Task<IEnumerable<UserTask>> GetDeletedByUserIdAsync(int  userId, CancellationToken cancellationToken);
    Task CreateAsync(UserTask userTask, CancellationToken cancellationToken);
    Task UpdateAsync(UpdateUserTaskRequest userTaskRequest, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task ChangeStatusAsync(int id, TaskStatus status, CancellationToken cancellationToken);
    Task RestoreAsync(int id, CancellationToken cancellationToken);
}