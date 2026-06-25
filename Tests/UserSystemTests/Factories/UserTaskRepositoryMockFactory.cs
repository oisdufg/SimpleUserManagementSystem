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
            .Returns(call =>
            {
                int userId = call.Arg<int>();
                List<UserTask> result = userTasks?
                    .Where(x => x.UserID == userId)
                    .ToList()
                    ?? new List<UserTask>();
                return Task.FromResult<IEnumerable<UserTask>>(result);
            });
        mock.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(userTasks ?? new List<UserTask>()));
        mock.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(call =>
            {
                int id = call.Arg<int>();
                return Task.FromResult(userTasks?.FirstOrDefault(x => x.ID == id) ?? userTask ?? new UserTask());
            });
        mock.GetByNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(userTasks ?? new List<UserTask>()));
        mock.GetDeletedAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(userTasks ?? new List<UserTask>()));
        mock.GetDeletedByUserIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(call =>
            {
                int userId = call.Arg<int>();
                List<UserTask> result = userTasks?
                    .Where(x => x.UserID == userId)
                    .ToList()
                    ?? new List<UserTask>();
                return Task.FromResult<IEnumerable<UserTask>>(result);
            });
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