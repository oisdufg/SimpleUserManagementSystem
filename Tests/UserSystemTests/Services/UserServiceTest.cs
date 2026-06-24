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
}