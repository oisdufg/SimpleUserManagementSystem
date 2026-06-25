using System.Diagnostics.CodeAnalysis;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using NSubstitute;
using NSubstitute.Core;
using SimpleUserManagementSystem.Common.Protos;
using UserSystemService.MappingProfiles;
using UserSystemService.Services;
using UserSystemTests.Factories;
using UserSystemTests.Mocks;
using User = UserSystemService.Models.User;
using UserTask = UserSystemService.Models.UserTask;

namespace UserSystemTests.Services;

[ExcludeFromCodeCoverage]
internal class UserServiceTest : BaseUnitTest
{
    private UserService _service;

    [Test]
    public async Task GetAll_should_return_all_users()
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
        IEnumerable<User> expected = [userOne,  usertTwo];

        InitializeMocks(users: expected);
        _service = new UserService(UserRepository);
        
        IServerStreamWriter<UserData> writer = GrpcMock.GetTestServerStreamWriter<UserData>();
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.GetAll));
        
        //Act
        await _service.GetAll(new Empty(), writer, context);
        
        //Assert
        IEnumerable<ICall> calls = UserRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserRepository.GetAllAsync));
        Assert.That(call, Is.Not.Null);

        foreach (User record in expected)
        {
            await writer.Received().WriteAsync(record.Map());
        }
    }

    [Test]
    public async Task GetById_should_get_user_by_id()
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
        
        InitializeMocks(users: new[] { userOne });
        _service = new UserService(UserRepository);
        
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.GetById));
        
        //Act
        UserData actual = await _service.GetById(new Int32Value { Value = 1 }, context);
        
        //Assert
        IEnumerable<ICall> calls = UserRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserRepository.GetByIdAsync));
        Assert.That(call, Is.Not.Null);
        
        Assert.That(actual, Is.Not.Null);
        Assert.That(actual, Is.EqualTo(userOne.Map()));
    }

    [Test]
    public async Task GetById_should_return_exception_when_user_not_found()
    {
        //Arrange
        InitializeMocks();
        _service = new UserService(UserRepository);
        
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.GetById));
        
        //Act
        KeyNotFoundException exception = Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.GetById(new Int32Value { Value = 1 }, context));
        
        //Assert
        IEnumerable<ICall> calls = UserRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserRepository.GetByIdAsync));
        Assert.That(call, Is.Not.Null);
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("User with id 1 not found"));
    }

    [Test]
    public async Task Create_should_create_user()
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
        
        InitializeMocks();
        _service = new UserService(UserRepository);
        
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.Create));
        
        //Act
        await _service.Create(userOne.Map(), context);
        
        //Assert
        IEnumerable<ICall> calls = UserRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserRepository.CreateAsync));
        Assert.That(call, Is.Not.Null);
    }
    
    [Test]
    public async Task Update_should_update_user()
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
        
        InitializeMocks();
        _service = new UserService(UserRepository);
        
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.Update));
        
        //Act
        await _service.Update(userOne.Map(), context);
        
        //Assert
        IEnumerable<ICall> calls = UserRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserRepository.UpdateAsync));
        Assert.That(call, Is.Not.Null);
    }

    [Test]
    public async Task Delete_should_delete_user()
    {
        //Arrange
        InitializeMocks();
        _service = new UserService(UserRepository);
        
        ServerCallContext context = GrpcMock.GetCallContext(nameof(_service.Delete));
        
        //Act
        await _service.Delete(new Int32Value { Value = 1 }, context);
        
        //Assert
        IEnumerable<ICall> calls = UserRepository.ReceivedCalls();
        ICall call = calls.FirstOrDefault(e => e.GetMethodInfo().Name == nameof(UserRepository.DeleteAsync));
        Assert.That(call, Is.Not.Null);
    }
}