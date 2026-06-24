using System.Diagnostics.CodeAnalysis;
using Grpc.Core;
using Grpc.Net.Client;
using NSubstitute;
using SimpleUserManagementSystem.Common.Protos;

namespace UserSystemTests.Mocks;

[ExcludeFromCodeCoverage]
public class GrpcMock
{
    public static GrpcChannel Channel { get; } = GrpcChannel.ForAddress("http://test.loc");

    public static ServerCallContext GetCallContext(string methodName)
    {
        ServerCallContext context = Substitute.For<ServerCallContext>();
        
        context.Method.Returns($"/TestService/{methodName}");
        context.Deadline.Returns(DateTime.UtcNow.AddMinutes(1));
        context.CancellationToken.Returns(CancellationToken.None);
        
        return context;
    }

    public static IServerStreamWriter<T> GetTestServerStreamWriter<T>()
    {
        IServerStreamWriter<T> writer = Substitute.For<IServerStreamWriter<T>>();
        writer.WriteAsync(Arg.Any<T>())
            .Returns(Task.CompletedTask);

        return writer;
    }
}