using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using UserSystemService.Context;
using UserSystemService.Interfaces;
using Pomelo.EntityFrameworkCore;
using NSubstitute;

namespace UserSystemTests.Mocks;

[ExcludeFromCodeCoverage]
internal class ApplicationDbContextMock
{
    public ApplicationDbContext Context { get; set; }
    public IApplicationDbContext Mock { get; set; }

    public ApplicationDbContextMock()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        Context = new ApplicationDbContext(options);

        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
        
        Mock = Substitute.For<IApplicationDbContext>();
        
        Mock.Users.Returns(Context.Users);
        Mock.UserTasks.Returns(Context.UserTasks);
        
        Mock.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(call => Context.SaveChangesAsync(call.Arg<CancellationToken>()));
        
        Mock.SaveChanges()
            .Returns(call => Context.SaveChanges());
    }
}