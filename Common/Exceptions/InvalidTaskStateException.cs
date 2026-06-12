using System.Diagnostics.CodeAnalysis;
using Grpc.Core;

namespace Exceptions;

[ExcludeFromCodeCoverage]
public sealed class InvalidTaskStateException: RpcException
{
    public InvalidTaskStateException(string message) : base(new Status(StatusCode.FailedPrecondition, message), message)
    {
    }
    
    public InvalidTaskStateException(string message, Metadata trailers) : base(new Status(StatusCode.FailedPrecondition, message), trailers, message)
    {
    }
}