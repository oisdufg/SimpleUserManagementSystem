using System.Diagnostics.CodeAnalysis;
using UserSystemService.Interfaces;
using UserSystemService.Models;
using UserSystemTests.Factories;

namespace UserSystemTests;

[ExcludeFromCodeCoverage]
internal class BaseUnitTest
{
    protected IUserRepository UserRepository { get; set; }
    protected IUserTaskRepository UserTaskRepository { get; set; }

    public void InitializeMocks(IEnumerable<User>? users = null, User? user = null, IEnumerable<UserTask>? userTasks = null, UserTask? userTask = null)
    {
        UserRepository = UserRepositoryMockFactory.Create(users, user);
        UserTaskRepository = UserTaskRepositoryMockFactory.Create(userTasks, userTask);
    }
}