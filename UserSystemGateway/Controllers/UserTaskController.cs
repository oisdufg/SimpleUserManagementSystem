using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using UserSystemGateway.MappingProfiles;
using SimpleUserManagementSystem.Common.Protos;
using UserSystemGateway.Models.DTO;

namespace UserSystemGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class UserTaskController : ControllerBase
{
    private readonly UserTask.UserTaskClient _client;
    public UserTaskController(UserTask.UserTaskClient client)
    {
        _client = client;
    }

    [HttpGet("user/{id}")]
    public async IAsyncEnumerable<UserTaskData> GetByUserIdAsync(int id, CancellationToken cancellationToken)
    {
        using AsyncServerStreamingCall<UserTaskData> call = 
            _client.GetByUserId(new Int32Value { Value = id }, cancellationToken: cancellationToken);
        await foreach (UserTaskData response in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            yield return response;
        }
    }
    
    [HttpGet]
    public async IAsyncEnumerable<UserTaskData> GetAllAsync(CancellationToken cancellationToken)
    {
        using AsyncServerStreamingCall<UserTaskData> call = 
            _client.GetAll(new Empty(), cancellationToken: cancellationToken);
        await foreach (UserTaskData response in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            yield return response;
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        UserTaskData? result = await _client.GetByIdAsync(new Int32Value { Value = id }, cancellationToken: cancellationToken);
        return Ok(result);
    }
    
    [HttpGet("name/{name}")]
    public async IAsyncEnumerable<UserTaskData> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        using AsyncServerStreamingCall<UserTaskData> call = 
            _client.GetByName(new StringValue { Value = name }, cancellationToken: cancellationToken);
        await foreach (UserTaskData response in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            yield return response;
        }
    }
    
    [HttpGet("deleted")]
    public async IAsyncEnumerable<UserTaskData> GetDeletedAsync(CancellationToken cancellationToken)
    {
        using AsyncServerStreamingCall<UserTaskData> call = 
            _client.GetDeleted(new Empty(), cancellationToken: cancellationToken);
        await foreach (UserTaskData response in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            yield return response;
        }
    }
    
    [HttpGet("deleted/user/{id}")]
    public async IAsyncEnumerable<UserTaskData> GetDeletedByUserIdAsync(int id, CancellationToken cancellationToken)
    {
        using AsyncServerStreamingCall<UserTaskData> call = 
            _client.GetDeletedByUserId(new Int32Value { Value = id }, cancellationToken: cancellationToken);
        await foreach (UserTaskData response in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            yield return response;
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserTaskRequest request, CancellationToken cancellationToken)
    {
        await _client.CreateAsync(request.Map(), cancellationToken: cancellationToken);
        return Ok();
    }

    [HttpPatch("{request.ID}")]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserTaskRequest request, CancellationToken cancellationToken)
    {
        await _client.UpdateAsync(request.Map(), cancellationToken: cancellationToken);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await _client.DeleteAsync(new Int32Value { Value = id }, cancellationToken: cancellationToken);
        return Ok();
    }

    [HttpPatch("{request.ID}/status")]
    public async Task<IActionResult> ChangeStatusAsync(ChangeTaskStatusRequest request, CancellationToken cancellationToken)
    {
        await _client.ChangeStatusAsync(request.Map(), cancellationToken: cancellationToken);
        return Ok();
    }

    [HttpPatch("{id}/restore")]
    public async Task<IActionResult> RestoreAsync(int id, CancellationToken cancellationToken)
    {
        await _client.RestoreAsync(new Int32Value { Value = id }, cancellationToken: cancellationToken);
        return Ok();
    }
}