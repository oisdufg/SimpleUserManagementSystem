using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using SimpleUserManagementSystem.Common.Protos;

namespace UserSystemGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly User.UserClient _client;
    
    public UserController(User.UserClient client)
    {
        _client = client;
    }
    
    [HttpGet]
    public async IAsyncEnumerable<UserData> GetAllAsync(CancellationToken cancellationToken)
    {
        using AsyncServerStreamingCall<UserData> call = _client.GetAll(new Empty());
        await foreach (UserData response in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            yield return response;
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        UserData response = await _client.GetByIdAsync(new Int32Value { Value = id }, cancellationToken: cancellationToken);
        return Ok(response);
    }

    /*[HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] Models.User request, CancellationToken cancellationToken)
    {
        await _client.CreateAsync(request, cancellationToken: cancellationToken)
    }*/
}