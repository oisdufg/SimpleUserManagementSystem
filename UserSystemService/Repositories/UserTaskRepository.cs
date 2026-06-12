using FuzzySharp;
using Microsoft.EntityFrameworkCore;
using SimpleUserManagementSystem.Common.Protos;
using UserSystemService.Context;
using UserSystemService.Interfaces;
using UserSystemService.Models;
using TaskStatus = UserSystemService.Enums.TaskStatus;
using UserTask = UserSystemService.Models.UserTask;

namespace UserSystemService.Repositories;

public class UserTaskRepository : IUserTaskRepository
{
    private readonly ApplicationDbContext _context;

    public UserTaskRepository(ApplicationDbContext context)
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

    public async Task<int> CreateAsync(UserTask userTask, CancellationToken cancellationToken)
    {
        userTask.CreatedAt = DateTime.UtcNow;
        userTask.ModifiedAt = DateTime.UtcNow;
        userTask.IsDeleted = false;
        userTask.TaskStatus = TaskStatus.ToDo;
        
        await _context.UserTasks.AddAsync(userTask, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return userTask.ID;
    }

    public async Task UpdateAsync(UpdateUserTask userTask, CancellationToken cancellationToken)
    {
        UserTask local = await GetByIdAsync(userTask.ID, cancellationToken);
        
        _context.Entry(local)
            .Property(x => x.RowVersion)
            .OriginalValue = userTask.RowVersion;

        local.UserID = userTask.UserID ?? local.UserID;
        local.TaskName = userTask.TaskName ?? local.TaskName;
        local.TaskDescription = userTask.TaskDescription ?? local.TaskDescription;
        local.ModifiedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new InvalidOperationException($"Task with id {userTask.ID} was modified by another user");
        }
        
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await CheckIfTaskExistsAsync(id, CancellationToken.None);
        
        await _context.UserTasks
            .Where(e => e.ID == id)
            .ExecuteUpdateAsync(e => e
                .SetProperty(p => p.IsDeleted, true)
                .SetProperty(p => p.ModifiedAt, DateTime.UtcNow),
                cancellationToken);
    }

    public async Task ChangeStatusAsync(int id, TaskStatus status, CancellationToken cancellationToken)
    {
        await CheckIfTaskExistsAsync(id, CancellationToken.None);
        
        await _context.UserTasks
            .Where(e => e.ID == id)
            .ExecuteUpdateAsync(e => e
                    .SetProperty(p => p.TaskStatus, status)
                    .SetProperty(p => p.ModifiedAt, DateTime.UtcNow),
                cancellationToken);
    }

    public async Task RestoreAsync(int id, CancellationToken cancellationToken)
    {
        await CheckIfTaskExistsAsync(id, CancellationToken.None);
        
        await _context.UserTasks
            .Where(e => e.ID == id)
            .ExecuteUpdateAsync(e => e
                    .SetProperty(p => p.IsDeleted, false)
                    .SetProperty(p => p.ModifiedAt, DateTime.UtcNow),
                cancellationToken);
        
    }

    private async Task CheckIfTaskExistsAsync(int id, CancellationToken cancellationToken)
    {
        bool data = await _context.UserTasks.AsNoTracking().AnyAsync(e => e.ID == id, cancellationToken);
        if (data is false)
        {
            throw new KeyNotFoundException($"Task with {id} not found");
        }
    }
}