using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.ExceptionExtensions;
using UserSystemService.Models;
using UserSystemService.Repositories;
using UserSystemTests.Mocks;
using TaskStatus = UserSystemService.Enums.TaskStatus;

namespace UserSystemTests.Repositories;

[ExcludeFromCodeCoverage]
internal class UserTaskRepositoryTest
{ 
    private UserTaskRepository _repository;
    private ApplicationDbContextMock _applicationDbContextMock;

    [Test]
    public async Task GetByUserIdAsync_should_get_user()
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
            UserID = 2,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        UserTask taskThree = new()
        {
            ID = 3,
            TaskName = "Foo3",
            TaskDescription = "Bar3",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        IEnumerable<UserTask> expected = [taskOne, taskThree];

        _applicationDbContextMock =  new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        _applicationDbContextMock.Context.UserTasks.Add(taskOne);
        _applicationDbContextMock.Context.UserTasks.Add(taskTwo);
        _applicationDbContextMock.Context.UserTasks.Add(taskThree);
        _applicationDbContextMock.Context.SaveChangesAsync();
        
        //Act
        IEnumerable<UserTask> actual = await _repository.GetByUserIdAsync(1, CancellationToken.None);
        
        //Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(expected, Is.EqualTo(actual));
    }
    
    [Test]
    public async Task GetAllAsync_should_get_all_tasks()
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
            UserID = 2,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        IEnumerable<UserTask> expected = [taskOne, taskTwo];

        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        _applicationDbContextMock.Context.UserTasks.Add(taskOne);
        _applicationDbContextMock.Context.UserTasks.Add(taskTwo);
        _applicationDbContextMock.Context.SaveChangesAsync();
        
        //Act
        IEnumerable<UserTask> actual = await _repository.GetAllAsync(CancellationToken.None);
        
        //Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public async Task GetByIdAsync_should_get_task()
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
            UserID = 2,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        UserTask expected = taskTwo;
        
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        _applicationDbContextMock.Context.UserTasks.Add(taskOne);
        _applicationDbContextMock.Context.UserTasks.Add(taskTwo);
        _applicationDbContextMock.Context.SaveChangesAsync();
        
        //Act
        UserTask actual = await _repository.GetByIdAsync(2, CancellationToken.None);
        
        //Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public async Task GetByNameAsync_should_get_by_similar_name()
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
            UserID = 2,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        IEnumerable<UserTask> expected = [taskOne, taskTwo];
        
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        _applicationDbContextMock.Context.UserTasks.Add(taskOne);
        _applicationDbContextMock.Context.UserTasks.Add(taskTwo);
        _applicationDbContextMock.Context.SaveChangesAsync();
        
        //Act
        IEnumerable<UserTask> actual = await _repository.GetByNameAsync("Foo", CancellationToken.None);
        
        //Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public async Task GetByNameAsync_should_return_null()
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
            UserID = 2,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        _applicationDbContextMock.Context.UserTasks.Add(taskOne);
        _applicationDbContextMock.Context.UserTasks.Add(taskTwo);
        _applicationDbContextMock.Context.SaveChangesAsync();
        
        //Act
        IEnumerable<UserTask> actual = await _repository.GetByNameAsync("Bar", CancellationToken.None);
        
        //Assert
        Assert.That(actual, Is.Empty);
    }
    
    [Test]
    public async Task GetDeletedAsync_should_get_deleted_tasks()
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
            IsDeleted = true,
            UserID = 2,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        IEnumerable<UserTask> expected = [taskTwo];
        
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        _applicationDbContextMock.Context.UserTasks.Add(taskOne);
        _applicationDbContextMock.Context.UserTasks.Add(taskTwo);
        _applicationDbContextMock.Context.SaveChangesAsync();
        
        //Act
        IEnumerable<UserTask> actual = await _repository.GetDeletedAsync(CancellationToken.None);
        
        //Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(expected, Is.EqualTo(actual));
    }
    
    [Test]
    public async Task GetDeletedByUserIdAsync_should_get_deleted_tasks_by_user_id()
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
        UserTask taskTwo = new()
        {
            ID = 2,
            TaskName = "Foo2",
            TaskDescription = "Bar2",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = true,
            UserID = 2,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        IEnumerable<UserTask> expected = [taskTwo];
        
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        _applicationDbContextMock.Context.UserTasks.Add(taskOne);
        _applicationDbContextMock.Context.UserTasks.Add(taskTwo);
        _applicationDbContextMock.Context.SaveChangesAsync();
        
        //Act
        IEnumerable<UserTask> actual = await _repository.GetDeletedByUserIdAsync(2, CancellationToken.None);
        
        //Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public async Task CreateAsync_should_create_task()
    {
        UserTask request = new()
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
        
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        //Act
        await _repository.CreateAsync(request, CancellationToken.None);
        
        //Assert
        IEnumerable<ICall> clientCalls = _applicationDbContextMock.Mock.ReceivedCalls();
        ICall saveCall = clientCalls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_applicationDbContextMock.Mock.SaveChangesAsync));
        Assert.That(saveCall, Is.Not.Null);
        
        Assert.That(request, Is.EqualTo(_applicationDbContextMock.Context.UserTasks.FirstOrDefault(e => e.ID == request.ID)));
    }

    [Test]
    public async Task UpdateAsync_should_update_task()
    {
        UserTask original = new()
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
        UserTask modified = new()
        {
            ID = 1,
            TaskName = "Foo2",
            TaskDescription = "Bar2",
            TaskStatus = TaskStatus.InProgress,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            IsDeleted = false,
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        UpdateUserTaskRequest request = new()
        {
            ID = 1,
            TaskName = "Foo2",
            TaskDescription = "Bar2",
            UserID = 1,
            RowVersion = new byte[] { 1, 2, 3 }
        };
        
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        _applicationDbContextMock.Context.UserTasks.Add(original);
        _applicationDbContextMock.Context.SaveChangesAsync();
        
        //Act
        await _repository.UpdateAsync(request, CancellationToken.None);
        
        //Assert
        IEnumerable<ICall> clientCalls = _applicationDbContextMock.Mock.ReceivedCalls();
        ICall saveCall = clientCalls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_applicationDbContextMock.Mock.SaveChangesAsync));
        Assert.That(saveCall, Is.Not.Null);
        
       Assert.That(modified.TaskName, Is.EqualTo(_applicationDbContextMock.Context.UserTasks.FirstOrDefault(e => e.ID == request.ID).TaskName));
    }

    [Test]
    public async Task UpdateAsync_should_return_not_found_exception()
    {
        //Arrange
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        //Act
        KeyNotFoundException exception = Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.UpdateAsync(new UpdateUserTaskRequest() { ID = 1 }, CancellationToken.None));
        
        //Assert
        IEnumerable<ICall> clientCalls = _applicationDbContextMock.Mock.ReceivedCalls();
        ICall saveCall = clientCalls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_applicationDbContextMock.Mock.SaveChangesAsync));
        Assert.That(saveCall, Is.Null);
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("Task with id 1 not found"));
    }

    /*[Test]
    public async Task UpdateAsync_should_return_invalid_operation_exception()
    {
        //Arrange
        UpdateUserTaskRequest request = new()
        {
            ID = 1,
            TaskName = "Foo2",
            TaskDescription = "Bar2",
            UserID = 1
        };

        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        _applicationDbContextMock.Context.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Throws(new DbUpdateConcurrencyException());
        
        //Act
        InvalidOperationException exception = Assert.ThrowsAsync<InvalidOperationException>(() => _repository.UpdateAsync(request, CancellationToken.None));
        
        //Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("Task with id 1 was modified by another user"));
    }*/

    [Test]
    public async Task DeleteAsync_should_delete_task()
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
        
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        _applicationDbContextMock.Context.UserTasks.Add(taskOne);
        _applicationDbContextMock.Context.SaveChangesAsync();
        
        //Act
        await _repository.DeleteAsync(1, CancellationToken.None);
        
        //Assert
        IEnumerable<ICall> clientCalls = _applicationDbContextMock.Mock.ReceivedCalls();
        ICall saveCall = clientCalls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_applicationDbContextMock.Mock.SaveChangesAsync));
        Assert.That(saveCall, Is.Not.Null);
        
        Assert.That(_applicationDbContextMock.Context.UserTasks.FirstOrDefault(e => e.ID == taskOne.ID), Is.Null);
    }

    [Test]
    public async Task DeleteAsync_should_return_not_found_exception()
    {
        //Arrange
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        //Act
        KeyNotFoundException exception = Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.DeleteAsync(1, CancellationToken.None));
        
        //Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("Task with id 1 not found"));
    }

    [Test]
    public async Task ChangeStatusAsync_should_change_status()
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
        
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        _applicationDbContextMock.Context.UserTasks.Add(taskOne);
        _applicationDbContextMock.Context.SaveChangesAsync();
        
        //Act
        await _repository.ChangeStatusAsync(1, TaskStatus.Cancelled,  CancellationToken.None);
        
        //Assert
        IEnumerable<ICall> clientCalls = _applicationDbContextMock.Mock.ReceivedCalls();
        ICall saveCall = clientCalls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_applicationDbContextMock.Mock.SaveChangesAsync));
        Assert.That(saveCall, Is.Not.Null);
        
        Assert.That(_applicationDbContextMock.Context.UserTasks.FirstOrDefault(e => e.ID == taskOne.ID).TaskStatus, Is.EqualTo(TaskStatus.Cancelled));
    }

    [Test]
    public async Task ChangeStatusAsync_should_throw_not_found_exception()
    {
        //Arrange
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        //Act
        KeyNotFoundException exception = Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.ChangeStatusAsync(1, TaskStatus.Cancelled, CancellationToken.None));
        
        //Assert
        IEnumerable<ICall> clientCalls = _applicationDbContextMock.Mock.ReceivedCalls();
        ICall saveCall = clientCalls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_applicationDbContextMock.Mock.SaveChangesAsync));
        Assert.That(saveCall, Is.Null);
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("Task with id 1 not found"));
    }

    [Test]
    public async Task RestoreAsync_should_restore_task()
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
        
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        _applicationDbContextMock.Context.UserTasks.Add(taskOne);
        _applicationDbContextMock.Context.SaveChangesAsync();
        
        //Act
        await _repository.RestoreAsync(1, CancellationToken.None);
        
        //Assert
        IEnumerable<ICall> clientCalls = _applicationDbContextMock.Mock.ReceivedCalls();
        ICall saveCall = clientCalls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_applicationDbContextMock.Mock.SaveChangesAsync));
        Assert.That(saveCall, Is.Not.Null);
        
        Assert.That(_applicationDbContextMock.Context.UserTasks.FirstOrDefault(e => e.ID == taskOne.ID).IsDeleted, Is.EqualTo(false));
    }

    [Test]
    public async Task RestoreAsync_should_throw_not_found_exception()
    {
        //Arrange
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserTaskRepository(_applicationDbContextMock.Mock);
        
        //Act
        KeyNotFoundException exception = Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.RestoreAsync(1, CancellationToken.None));
        
        //Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("Task with id 1 not found"));
    }
}