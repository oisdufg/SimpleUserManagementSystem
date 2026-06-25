using System.Diagnostics.CodeAnalysis;
using Exceptions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NSubstitute;
using NSubstitute.Core;
using SimpleUserManagementSystem.Common.Protos;
using UserSystemService.MappingProfiles;
using UserSystemService.Services;
using UserSystemTests.Mocks;
using UserTask = UserSystemService.Models.UserTask;
using TaskStatus = UserSystemService.Enums.TaskStatus;

namespace UserSystemTests.Services;

[ExcludeFromCodeCoverage]
internal class UserTaskServiceTest : BaseUnitTest
{
    private UserTaskService _service;
    
    [Test]
    public async Task GetByUserId_should_get_tasks_by_user_id()
    {
        //Arrange
        UserTask taskOne = new()
        {
            ID = 1,
            TaskName = "Foo1",
            TaskDescription = "Bar1",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        UserTask taskTwo = new()
        {
            ID = 2,
            TaskName = "Foo2",
            TaskDescription = "Bar2",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        IEnumerable<UserTask> expected = [taskOne,  taskTwo];
        
        InitializeMocks(userTasks: expected);
        _service = new UserTaskService(UserTaskRepository);
        
        IServerStreamWriter<UserTaskData> writer = GrpcMock.GetTestServerStreamWriter<UserTaskData>();
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.GetByUserId));
        
        //Act
        await _service.GetByUserId(new Int32Value { Value = 1 }, writer, context);
        
        //Assert
        IEnumerable<ICall> calls = UserTaskRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserTaskRepository.GetByUserIdAsync));
        Assert.That(call, Is.Not.Null);
        
        foreach (UserTask record in expected)
        {
            await writer.Received().WriteAsync(record.Map());
        }
    }
    
    
    [Test]
    public async Task GetAll_should_get_all_tasks()
    {
        //Arrange
        UserTask taskOne = new()
        {
            ID = 1,
            TaskName = "Foo1",
            TaskDescription = "Bar1",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        UserTask taskTwo = new()
        {
            ID = 2,
            TaskName = "Foo2",
            TaskDescription = "Bar2",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        IEnumerable<UserTask> expected = [taskOne,  taskTwo];
        
        InitializeMocks(userTasks: expected);
        _service = new UserTaskService(UserTaskRepository);
        
        IServerStreamWriter<UserTaskData> writer = GrpcMock.GetTestServerStreamWriter<UserTaskData>();
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.GetAll));
        
        //Act
        await _service.GetAll(new Empty(), writer, context);
        
        //Assert
        IEnumerable<ICall> calls = UserTaskRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserTaskRepository.GetAllAsync));
        Assert.That(call, Is.Not.Null);
        
        foreach (UserTask record in expected)
        {
            await writer.Received().WriteAsync(record.Map());
        }
    }
    
    [Test]
    public async Task GetById_should_get_task_by_id()
    {
        //Arrange
        UserTask taskOne = new()
        {
            ID = 1,
            TaskName = "Foo1",
            TaskDescription = "Bar1",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        UserTask taskTwo = new()
        {
            ID = 2,
            TaskName = "Foo2",
            TaskDescription = "Bar2",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        IEnumerable<UserTask> expected = [taskOne,  taskTwo];
        
        InitializeMocks(userTasks: expected);
        _service = new UserTaskService(UserTaskRepository);
        
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.GetById));
        
        //Act
        UserTaskData actual =  await _service.GetById(new Int32Value { Value = 1 }, context);
        
        //Assert
        IEnumerable<ICall> calls = UserTaskRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserTaskRepository.GetByIdAsync));
        Assert.That(call, Is.Not.Null);
        
        Assert.That(actual, Is.Not.Null);
        Assert.That(taskOne, Is.EqualTo(actual.Map()));
    }

    [Test]
    public async Task GetByName_should_get_task_by_name()
    {
        //Arrange
        InitializeMocks();
        _service = new UserTaskService(UserTaskRepository);
        
        IServerStreamWriter<UserTaskData> writer = GrpcMock.GetTestServerStreamWriter<UserTaskData>();
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.GetByName));
        
        //Act
        await _service.GetByName(new StringValue { Value = "Foo" }, writer, context);
        
        //Assert
        IEnumerable<ICall> calls = UserTaskRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserTaskRepository.GetByNameAsync));
        Assert.That(call, Is.Not.Null);
    }

    [Test]
    public async Task GetDeleted_should_get_deleted_tasks()
    {
        UserTask taskOne = new()
        {
            ID = 1,
            TaskName = "Foo1",
            TaskDescription = "Bar1",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        UserTask taskTwo = new()
        {
            ID = 2,
            TaskName = "Foo2",
            TaskDescription = "Bar2",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        IEnumerable<UserTask> expected = [taskOne,  taskTwo];
        
        InitializeMocks(userTasks: expected);
        _service = new UserTaskService(UserTaskRepository);
        
        IServerStreamWriter<UserTaskData> writer = GrpcMock.GetTestServerStreamWriter<UserTaskData>();
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.GetDeleted));
        
        //Act
        await _service.GetDeleted(new Empty(), writer, context);
        
        //Assert
        IEnumerable<ICall> calls = UserTaskRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserTaskRepository.GetDeletedAsync));
        Assert.That(call, Is.Not.Null);
    }
    
    [Test]
    public async Task GetDeletedByUserId_should_get_task_by_id()
    {
        //Arrange
        InitializeMocks();
        _service = new UserTaskService(UserTaskRepository);
        
        IServerStreamWriter<UserTaskData> writer = GrpcMock.GetTestServerStreamWriter<UserTaskData>();
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.GetDeletedByUserId));
        
        //Act
        await _service.GetDeletedByUserId(new Int32Value { Value = 1 }, writer, context);
        
        //Assert
        IEnumerable<ICall> calls = UserTaskRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserTaskRepository.GetDeletedByUserIdAsync));
        Assert.That(call, Is.Not.Null);
    }

    [Test]
    public async Task Create_should_create_task()
    {
        //Arrange
        InitializeMocks();
        _service = new UserTaskService(UserTaskRepository);
        
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.Create));
        
        //Act
        await _service.Create(new CreateUserTaskData { TaskName = "Foo1", TaskDescription = "Bar1" }, context);
        
        //Assert
        IEnumerable<ICall> calls = UserTaskRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserTaskRepository.CreateAsync));
        Assert.That(call, Is.Not.Null);
    }
    
    [Test]
    public async Task Update_should_update_task()
    {
        //Arrange
        UserTask taskOne = new()
        {
            ID = 1,
            TaskName = "Foo1",
            TaskDescription = "Bar1",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        
        InitializeMocks(userTask: taskOne);
        _service = new UserTaskService(UserTaskRepository);
        
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.Update));
        
        //Act
        await _service.Update(new UpdateUserTaskData { Id = 1, TaskName = "Foo1", TaskDescription = "Bar1" }, context);
        
        //Assert
        IEnumerable<ICall> calls = UserTaskRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserTaskRepository.UpdateAsync));
        Assert.That(call, Is.Not.Null);
    }

    [Test]
    public async Task Update_should_throw_exception()
    {
        //Arrange
        UserTask taskOne = new()
        {
            ID = 1,
            TaskName = "Foo1",
            TaskDescription = "Bar1",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = true,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        
        InitializeMocks(userTask: taskOne);
        _service = new UserTaskService(UserTaskRepository);
        
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.Update));
        
        //Act
        InvalidTaskStateException exception = Assert.ThrowsAsync<InvalidTaskStateException>(() =>
            _service.Update(new UpdateUserTaskData { Id = 1, TaskName = "Foo", TaskDescription = "Bar"}, context));
        
        //Assert
        IEnumerable<ICall> calls = UserTaskRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserTaskRepository.UpdateAsync));
        Assert.That(call, Is.Null);
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("Task with id 1 is deleted and cannot be updated"));
        
    }

    [Test]
    public async Task Delete_should_delete_task()
    {
        //Arrange
        UserTask taskOne = new()
        {
            ID = 1,
            TaskName = "Foo1",
            TaskDescription = "Bar1",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        
        InitializeMocks(userTask: taskOne);
        _service = new UserTaskService(UserTaskRepository);
        
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.Delete));
        
        //Act
        await _service.Delete(new Int32Value { Value = 1 }, context);
        
        //Assert
        IEnumerable<ICall> calls = UserTaskRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserTaskRepository.DeleteAsync));
        Assert.That(call, Is.Not.Null);
    }

    [Test]
    public async Task Delete_should_throw_exception()
    {
        //Arrange
        UserTask taskOne = new()
        {
            ID = 1,
            TaskName = "Foo1",
            TaskDescription = "Bar1",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = true,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        
        InitializeMocks(userTask: taskOne);
        _service = new UserTaskService(UserTaskRepository);
        
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.Delete));
        
        //Act
        InvalidTaskStateException exception = Assert.ThrowsAsync<InvalidTaskStateException>(() =>
            _service.Delete(new Int32Value { Value = 1 }, context));
        
        //Assert
        IEnumerable<ICall> calls = UserTaskRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserTaskRepository.DeleteAsync));
        Assert.That(call, Is.Null);
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("Task with id 1 is already deleted"));
    }
    
    [Test]
    public async Task ChangeStatus_should_change_status()
    {
        //Arrange
        UserTask taskOne = new()
        {
            ID = 1,
            TaskName = "Foo1",
            TaskDescription = "Bar1",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        
        InitializeMocks(userTask: taskOne);
        _service = new UserTaskService(UserTaskRepository);
        
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.ChangeStatus));
        
        //Act
        await _service.ChangeStatus(new ChangeStatusData { Id = 1, 
            TaskStatus = SimpleUserManagementSystem.Common.Protos.TaskStatus.Cancelled}, context);
        
        //Assert
        IEnumerable<ICall> calls = UserTaskRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserTaskRepository.ChangeStatusAsync));
        Assert.That(call, Is.Not.Null);
    }

    [Test]
    public async Task ChangeStatus_should_throw_exception()
    {
        UserTask taskOne = new()
        {
            ID = 1,
            TaskName = "Foo1",
            TaskDescription = "Bar1",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = true,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        
        InitializeMocks(userTask: taskOne);
        _service = new UserTaskService(UserTaskRepository);
        
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.ChangeStatus));
        
        //Act
        InvalidTaskStateException exception = Assert.ThrowsAsync<InvalidTaskStateException>(() =>
            _service.ChangeStatus(new ChangeStatusData { Id = 1, 
                TaskStatus = SimpleUserManagementSystem.Common.Protos.TaskStatus.Cancelled}, context));
        
        //Assert
        IEnumerable<ICall> calls = UserTaskRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserTaskRepository.ChangeStatusAsync));
        Assert.That(call, Is.Null);
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("Task with id 1 is deleted and cannot be updated"));
    }
    
    [Test]
    public async Task Restore_should_restore_task()
    {
        //Arrange
        UserTask taskOne = new()
        {
            ID = 1,
            TaskName = "Foo1",
            TaskDescription = "Bar1",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = true,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        
        InitializeMocks(userTask: taskOne);
        _service = new UserTaskService(UserTaskRepository);
        
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.Restore));
        
        //Act
        await _service.Restore(new Int32Value { Value = 1 }, context);
        
        //Assert
        IEnumerable<ICall> calls = UserTaskRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserTaskRepository.RestoreAsync));
        Assert.That(call, Is.Not.Null);
    }

    [Test]
    public async Task Restore_should_throw_exception()
    {
        //Arrange
        UserTask taskOne = new()
        {
            ID = 1,
            TaskName = "Foo1",
            TaskDescription = "Bar1",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        
        InitializeMocks(userTask: taskOne);
        _service = new UserTaskService(UserTaskRepository);
        
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.Restore));
        
        //Act
        InvalidTaskStateException exception = Assert.ThrowsAsync<InvalidTaskStateException>(() => 
            _service.Restore(new Int32Value { Value = 1 }, context));
        
        //Assert
        IEnumerable<ICall> calls = UserTaskRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserTaskRepository.RestoreAsync));
        Assert.That(call, Is.Null);
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("Task with id 1 is not deleted"));
    }
}