using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using UserSystemService.Interfaces;
using UserSystemService.Models;
using TaskStatus = UserSystemService.Enums.TaskStatus;

namespace UserSystemTests.Factories;

[ExcludeFromCodeCoverage]
internal static class UserTaskRepositoryMockFactory
{
    public static IUserTaskRepository Create(IEnumerable<UserTask> userTasks = null, UserTask userTask = null)
    {
        IUserTaskRepository mock = Substitute.For<IUserTaskRepository>();

        mock.GetByUserIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(userTasks ?? []);
        mock.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(userTasks ?? []);
        mock.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(userTask ?? new UserTask());
        mock.GetByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(userTasks ?? []);
        mock.GetDeletedAsync(Arg.Any<CancellationToken>())
            .Returns(userTasks ?? []);
        mock.GetDeletedByUserIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(userTasks ?? []);
        mock.CreateAsync(Arg.Any<UserTask>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        mock.UpdateAsync(Arg.Any<UpdateUserTaskRequest>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        mock.DeleteAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        mock.ChangeStatusAsync(Arg.Any<int>(), Arg.Any<TaskStatus>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        mock.RestoreAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        return mock;
    }
}