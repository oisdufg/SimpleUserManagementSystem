using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NSubstitute.Core;
using SimpleUserManagementSystem.Common.Protos;
using UserSystemService.Models.DTO;
using UserSystemService.Repositories;
using UserSystemTests.Mocks;
using User = UserSystemService.Models.User;
using UserTask = UserSystemService.Models.UserTask;

namespace UserSystemTests.Repositories;

[ExcludeFromCodeCoverage]
internal class UserRepositoryTest
{
    private UserRepository _repository;
    private ApplicationDbContextMock _applicationDbContextMock;

    [Test]
    public async Task GetAllAsync_should_get_all_users()
    {
        //Arrange
        User userOne = new User()
        {
            ID = 1,
            FirstName = "Foo1",
            MiddleName = "Bar1",
            LastName = "Baz1",
            Birthday = new DateTime(1990, 1, 1),
            Email = "foo1@mail.com",
            Tasks  = new List<UserTask>()
        };
        User usertTwo = new User()
        {
            ID = 2,
            FirstName = "Foo2",
            MiddleName = "Bar2",
            LastName = "Baz2",
            Birthday = new DateTime(1990, 2, 2),
            Email = "foo2@mail.com",
            Tasks  = new List<UserTask>()
        };

        IEnumerable<User> expected = [userOne, usertTwo];
        
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserRepository(_applicationDbContextMock.Mock);
        
        _applicationDbContextMock.Mock.Users.Add(userOne);
        _applicationDbContextMock.Mock.Users.Add(usertTwo);
        await _applicationDbContextMock.Context.SaveChangesAsync();
        
        //Act
        IEnumerable<User> actual = await _repository.GetAllAsync(CancellationToken.None);
        
        //Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public async Task GetAllUserNamesAsync_should_get_all_user_names()
    {
        User userOne = new()
        {
            ID = 1,
            FirstName = "Foo1",
            MiddleName = "Bar1",
            LastName = "Baz1",
            Birthday = new DateTime(1990, 1, 1),
            Email = "foo1@mail.com",
            Tasks  = new List<UserTask>()
        };
        User usertTwo = new()
        {
            ID = 2,
            FirstName = "Foo2",
            MiddleName = "Bar2",
            LastName = "Baz2",
            Birthday = new DateTime(1990, 2, 2),
            Email = "foo2@mail.com",
            Tasks  = new List<UserTask>()
        };
        UserShortInfoRequest userOneShortInfo = new()
        {
            ID = userOne.ID,
            FullName = userOne.FirstName + " " + userOne.LastName + " " + userOne.MiddleName
        };
        UserShortInfoRequest userTwoShortInfo = new()
        {
            ID = usertTwo.ID,
            FullName = usertTwo.FirstName + " " + usertTwo.LastName + " " + usertTwo.MiddleName
        };
        
        IEnumerable<UserShortInfoRequest> expected = [userOneShortInfo, userTwoShortInfo];
        
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserRepository(_applicationDbContextMock.Mock);
        
        _applicationDbContextMock.Mock.Users.Add(userOne);
        _applicationDbContextMock.Mock.Users.Add(usertTwo);
        await _applicationDbContextMock.Context.SaveChangesAsync();
        
        //Act
        IEnumerable<UserShortInfoRequest> actual = await _repository.GetAllUserNamesAsync(CancellationToken.None);
        
        //Assert
        Assert.That(actual, Is.Not.Null);
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public async Task GetByIdAsync_should_return_user()
    {
       //Arrange
       User userOne = new()
       {
           ID = 1,
           FirstName = "Foo1",
           MiddleName = "Bar1",
           LastName = "Baz1",
           Birthday = new DateTime(1990, 1, 1),
           Email = "foo1@mail.com",
           Tasks  = new List<UserTask>()
       };
       User usertTwo = new()
       {
           ID = 2,
           FirstName = "Foo2",
           MiddleName = "Bar2",
           LastName = "Baz2",
           Birthday = new DateTime(1990, 2, 2),
           Email = "foo2@mail.com",
           Tasks  = new List<UserTask>()
       };
       
       User expected = usertTwo;
       _applicationDbContextMock = new ApplicationDbContextMock();
       _repository = new UserRepository(_applicationDbContextMock.Mock);
       
       _applicationDbContextMock.Mock.Users.Add(userOne);
       _applicationDbContextMock.Mock.Users.Add(usertTwo);
       await _applicationDbContextMock.Context.SaveChangesAsync();
       
       //Act
       User actual = await _repository.GetByIdAsync(2, CancellationToken.None);
       
       //Assert
       Assert.That(actual, Is.Not.Null);
       Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public async Task GetByIdAsync_should_return_not_found_exception()
    {
        //Arrange
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserRepository(_applicationDbContextMock.Mock);
        
        //Act
        KeyNotFoundException exception = Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.GetByIdAsync(1, CancellationToken.None));
        
        //Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("User with id 1 not found"));
    }

    [Test]
    public async Task CreateAsync_should_create_user()
    {
        //Arrange
        User request = new()
        {
            ID = 1,
            FirstName = "Foo1",
            MiddleName = "Bar1",
            LastName = "Baz1",
            Birthday = new DateTime(1990, 1, 1),
            Email = "foo1@mail.com",
            Tasks  = new List<UserTask>()
        };
        
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserRepository(_applicationDbContextMock.Mock);
        
        //Act
        await _repository.CreateAsync(request, CancellationToken.None);
        
        //Assert
        IEnumerable<ICall> clientCalls = _applicationDbContextMock.Mock.ReceivedCalls();
        ICall saveCall = clientCalls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_applicationDbContextMock.Mock.SaveChangesAsync));
        Assert.That(saveCall, Is.Not.Null);
    }
    
    [Test]
    public async Task UpdateAsync_should_update_user()
    {
        //Arrange
        User original = new()
        {
            ID = 1,
            FirstName = "Foo1",
            MiddleName = "Bar1",
            LastName = "Baz1",
            Birthday = new DateTime(1990, 1, 1),
            Email = "foo1@mail.com",
            Tasks  = new List<UserTask>()
        };
        User changed = new()
        {
            ID = 1,
            FirstName = "Foo2",
            MiddleName = "Bar2",
            LastName = "Baz2",
            Birthday = new DateTime(1990, 2, 2),
            Email = "foo2@mail.com",
            Tasks  = new List<UserTask>()
        };
        
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserRepository(_applicationDbContextMock.Mock);
        
        _applicationDbContextMock.Mock.Users.Add(original);
        await _applicationDbContextMock.Context.SaveChangesAsync();
        
        //Act
        await _repository.UpdateAsync(changed, CancellationToken.None);
        
        //Assert
        IEnumerable<ICall> clientCalls = _applicationDbContextMock.Mock.ReceivedCalls();
        ICall saveCall = clientCalls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_applicationDbContextMock.Mock.SaveChangesAsync));
        Assert.That(saveCall, Is.Not.Null);
        
        Assert.That(_applicationDbContextMock.Context.Users, Has.Some.EqualTo(changed));
    }

    [Test]
    public async Task UpdateAsync_should_throw_not_found_exception()
    {
        //Arrange
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserRepository(_applicationDbContextMock.Mock);
        
        //Act
        KeyNotFoundException exception = Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.UpdateAsync(new User() { ID = 1 }, CancellationToken.None));
        
        //Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("User with id 1 not found"));
    }

    [Test]
    public async Task DeleteAsync_should_delete_user()
    {
        //Arrange
        User request = new()
        {
            ID = 1,
            FirstName = "Foo1",
            MiddleName = "Bar1",
            LastName = "Baz1",
            Birthday = new DateTime(1990, 1, 1),
            Email = "foo1@mail.com",
            Tasks  = new List<UserTask>()
        };
        
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserRepository(_applicationDbContextMock.Mock);
        
        _applicationDbContextMock.Mock.Users.Add(request);
        await _applicationDbContextMock.Context.SaveChangesAsync();
        
        //Act
        await _repository.DeleteAsync(1,  CancellationToken.None);
        
        //Arrange
        IEnumerable<ICall> clientCalls = _applicationDbContextMock.Mock.ReceivedCalls();
        ICall saveCall = clientCalls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(_applicationDbContextMock.Mock.SaveChangesAsync));
        Assert.That(saveCall, Is.Not.Null);
        
        User actual = _applicationDbContextMock.Context.Users.FirstOrDefault(e => e.ID == request.ID);
        Assert.That(actual, Is.Null);
    }

    [Test]
    public async Task DeleteAsync_should_throw_not_found_exception()
    {
        //Arrange
        _applicationDbContextMock = new ApplicationDbContextMock();
        _repository = new UserRepository(_applicationDbContextMock.Mock);
        
        //Act
        KeyNotFoundException exception = Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.DeleteAsync(1, CancellationToken.None));
        
        //Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("User with id 1 not found"));
    }
}