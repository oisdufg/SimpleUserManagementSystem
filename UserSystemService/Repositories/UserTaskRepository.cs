using FuzzySharp;
using Microsoft.EntityFrameworkCore;
using UserSystemService.Context;
using UserSystemService.Interfaces;
using UserSystemService.Models;
using TaskStatus = UserSystemService.Enums.TaskStatus;
using UserTask = UserSystemService.Models.UserTask;

namespace UserSystemService.Repositories;

public class UserTaskRepository : IUserTaskRepository
{
    private readonly IApplicationDbContext _context;

    public UserTaskRepository(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<UserTask>> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.UserTasks
            .AsNoTracking()
            .Where(e => e.UserID == userId && e.IsDeleted == false)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserTask>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.UserTasks
            .AsNoTracking()
            .Where(e => e.IsDeleted == false)
            .ToListAsync(cancellationToken);
    }

    public async Task<UserTask> GetByIdAsync(int taskId, CancellationToken cancellationToken)
    {
        return await _context.UserTasks
                   .AsNoTracking()
                   .FirstOrDefaultAsync(e => e.ID == taskId, cancellationToken) ??
               throw new KeyNotFoundException($"Task with {taskId} not found");
    }

    public async Task<IEnumerable<UserTask>> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        IEnumerable<UserTask> tasks = await _context.UserTasks
            .AsNoTracking()
            .Where(e => e.IsDeleted == false)
            .ToListAsync(cancellationToken);

        return tasks
            .Select(e => new
            {
                Task = e,
                Score = Fuzz.PartialRatio(
                    e.TaskName,
                    name)
            })
            .Where(e => e.Score >= 70)
            .OrderByDescending(e => e.Score)
            .Select(e => e.Task);
    }

    public async Task<IEnumerable<UserTask>> GetDeletedAsync(CancellationToken cancellationToken)
    {
        return await _context.UserTasks
            .AsNoTracking()
            .Where(e => e.IsDeleted == true)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserTask>> GetDeletedByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        return await _context.UserTasks
            .AsNoTracking()
            .Where(e => e.UserID == userId && e.IsDeleted == true)
            .ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(UserTask userTask, CancellationToken cancellationToken)
    {
        userTask.CreatedAt = DateTime.UtcNow;
        userTask.ModifiedAt = DateTime.UtcNow;
        userTask.IsDeleted = false;
        userTask.TaskStatus = TaskStatus.ToDo;
        
        await _context.UserTasks.AddAsync(userTask, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UpdateUserTaskRequest userTaskRequest, CancellationToken cancellationToken)
    {
        UserTask? entity = await _context.UserTasks
            .FirstOrDefaultAsync(x => x.ID == userTaskRequest.ID, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException($"Task with id {userTaskRequest.ID} not found");
        
        entity.RowVersion = userTaskRequest.RowVersion;
        entity.UserID = userTaskRequest.UserID ?? entity.UserID;
        entity.TaskName = userTaskRequest.TaskName ?? entity.TaskName;
        entity.TaskDescription = userTaskRequest.TaskDescription ?? entity.TaskDescription;
        entity.ModifiedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new InvalidOperationException(
                $"Task with id {userTaskRequest.ID} was modified by another user");
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await CheckIfTaskExistsAsync(id, CancellationToken.None);
        
        UserTask userTask = await _context.UserTasks.FindAsync(id, cancellationToken);

        userTask.IsDeleted = true;
        userTask.ModifiedAt = DateTime.UtcNow;
        
        _context.UserTasks.Remove(userTask);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangeStatusAsync(int id, TaskStatus status, CancellationToken cancellationToken)
    {
        await CheckIfTaskExistsAsync(id, CancellationToken.None);
        
        UserTask entity = await _context.UserTasks.FirstAsync(e => e.ID == id, cancellationToken);
        
        entity.TaskStatus = status;
        entity.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RestoreAsync(int id, CancellationToken cancellationToken)
    {
        await CheckIfTaskExistsAsync(id, CancellationToken.None);
        
        UserTask entity = await _context.UserTasks.FirstAsync(e => e.ID == id, cancellationToken);
        
        entity.IsDeleted = false;
        entity.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task CheckIfTaskExistsAsync(int id, CancellationToken cancellationToken)
    {
        bool data = await _context.UserTasks.AsNoTracking().AnyAsync(e => e.ID == id, cancellationToken);
        if (data is false)
        {
            throw new KeyNotFoundException($"Task with id {id} not found");
        }
    }
}