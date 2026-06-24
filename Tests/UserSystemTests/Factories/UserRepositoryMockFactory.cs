using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using UserSystemService.Interfaces;
using UserSystemService.Models;

namespace UserSystemTests.Factories;

[ExcludeFromCodeCoverage]
internal static class UserRepositoryMockFactory
{
    public static IUserRepository Create(IEnumerable<User> users = null, User? user = null)
    {
        IUserRepository mock = Substitute.For<IUserRepository>();

        mock.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(users ?? []);
        mock.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(user ?? new User());
        mock.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        mock.UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        mock.DeleteAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        return mock;
    }
}