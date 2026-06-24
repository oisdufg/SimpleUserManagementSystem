using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using SimpleUserManagementSystem.Common.Protos;
using UserSystemGateway.MappingProfiles;

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

    [HttpGet("/usernames")]
    public async IAsyncEnumerable<UserNameData> GetAllUserNamesAsync(CancellationToken cancellationToken)
    {
        using AsyncServerStreamingCall<UserNameData> call = _client.GetAllUserNames(new Empty());
        await foreach (UserNameData response in call.ResponseStream.ReadAllAsync(cancellationToken))
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

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] Models.User request, CancellationToken cancellationToken)
    {
        await _client.CreateAsync(request.Map(), cancellationToken: cancellationToken);
        return Ok();
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateAsync([FromBody] Models.User request, CancellationToken cancellationToken)
    {
        await _client.UpdateAsync(request.Map(), cancellationToken: cancellationToken);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _client.DeleteAsync(new Int32Value { Value = id }, cancellationToken: cancellationToken);
        return Ok();
    }
}