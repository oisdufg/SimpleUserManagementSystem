using Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;
using SimpleUserManagementSystem.Common.Protos;
using UserSystemService.Interfaces;
using UserSystemService.MappingProfiles;
using TaskStatus = UserSystemService.Enums.TaskStatus;

namespace UserSystemService.Services;

public class UserTaskService : UserTask.UserTaskBase
{
    private readonly IUserTaskRepository _repository;
    
    public UserTaskService(IUserTaskRepository repository)
    {
        _repository = repository;
    }

    public override async Task GetByUserId(Int32Value request, IServerStreamWriter<UserTaskData> responseStream, ServerCallContext context)
    {
        IEnumerable<Models.UserTask> data = await _repository.GetByUserIdAsync(request.Value, context.CancellationToken);

        IEnumerable<UserTaskData> reply = data.Select(e => e.Map());
        await responseStream.WriteAllAsync(reply);
    }

    public override async Task GetAll(Empty request, IServerStreamWriter<UserTaskData> responseStream, ServerCallContext context)
    {
        IEnumerable<Models.UserTask> data = await _repository.GetAllAsync(context.CancellationToken);

        IEnumerable<UserTaskData> reply = data.Select(e => e.Map());
        await responseStream.WriteAllAsync(reply);
    }

    public override async Task<UserTaskData> GetById(Int32Value request, ServerCallContext context)
    {
        Models.UserTask data = await _repository.GetByIdAsync(request.Value, context.CancellationToken);
        return data.Map();
    }

    public override async Task GetByName(StringValue request, IServerStreamWriter<UserTaskData> responseStream, ServerCallContext context)
    {
        IEnumerable<Models.UserTask> data = await _repository.GetByNameAsync(request.Value, context.CancellationToken);

        IEnumerable<UserTaskData> reply = data.Select(e => e.Map());
        await responseStream.WriteAllAsync(reply);
    }

    public override async Task GetDeleted(Empty request, IServerStreamWriter<UserTaskData> responseStream, ServerCallContext context)
    {
        IEnumerable<Models.UserTask> data = await _repository.GetDeletedAsync(context.CancellationToken);

        IEnumerable<UserTaskData> reply = data.Select(e => e.Map());
        await responseStream.WriteAllAsync(reply);
    }

    public override async Task GetDeletedByUserId(Int32Value request, IServerStreamWriter<UserTaskData> responseStream, ServerCallContext context)
    {
        IEnumerable<Models.UserTask> data = await _repository.GetDeletedByUserIdAsync(request.Value, context.CancellationToken);

        IEnumerable<UserTaskData> reply = data.Select(e => e.Map());
        await responseStream.WriteAllAsync(reply);
    }

    public override async Task<Empty> Create(CreateUserTaskData request, ServerCallContext context)
    {
        await _repository.CreateAsync(request.Map(), context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> Update(UpdateUserTaskData request, ServerCallContext context)
    {
        Models.UserTask currentTask = await GetCurrentUserTaskAsync(request.Id, context);
        if (currentTask.IsDeleted is true)
        {
            throw new InvalidTaskStateException($"Task with id {currentTask.ID} is deleted and cannot be updated");
        }
        
        await _repository.UpdateAsync(request.Map(), context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> Delete(Int32Value request, ServerCallContext context)
    {
        Models.UserTask currentTask = await GetCurrentUserTaskAsync(request.Value, context);
        if (currentTask.IsDeleted is true)
        {
            throw new InvalidTaskStateException($"Task with id {currentTask.ID} is already deleted");
        }
        
        await _repository.DeleteAsync(request.Value, context.CancellationToken);

        return new Empty();
    }

    public override async Task<Empty> ChangeStatus(ChangeStatusData request, ServerCallContext context)
    {
        Models.UserTask currentTask = await GetCurrentUserTaskAsync(request.Id, context);
        if (currentTask.IsDeleted is true)
        {
            throw new InvalidTaskStateException($"Task with id {currentTask.ID} is deleted and cannot be updated");
        }

        await _repository.ChangeStatusAsync(request.Id, (TaskStatus)request.TaskStatus, context.CancellationToken);
        
        return new Empty();
    }

    public override async Task<Empty> Restore(Int32Value request, ServerCallContext context)
    {
        Models.UserTask currentTask = await GetCurrentUserTaskAsync(request.Value, context);
        if (currentTask.IsDeleted is false)
        {
            throw new InvalidTaskStateException($"Task with id {currentTask.ID} is not deleted");
        }
        
        await _repository.RestoreAsync(request.Value, context.CancellationToken);
        
        return new Empty();
    }

    private async Task<Models.UserTask> GetCurrentUserTaskAsync(int id, ServerCallContext context)
    {
        return await _repository.GetByIdAsync(id, context.CancellationToken);
    }
}