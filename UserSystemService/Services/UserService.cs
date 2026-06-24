using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;
using SimpleUserManagementSystem.Common.Protos;
using UserSystemService.Interfaces;
using UserSystemService.MappingProfiles;
using User = SimpleUserManagementSystem.Common.Protos.User;

namespace UserSystemService.Services;

public class UserService : User.UserBase
{
    private readonly IUserRepository _repository;
    
    public UserService(IUserRepository repository)
    {
        _repository = repository; 
    }
    
    public override async Task GetAll(Empty request, IServerStreamWriter<UserData> responseStream, ServerCallContext context)
    {
        IEnumerable<Models.User> data = await _repository.GetAllAsync(context.CancellationToken);
        
        IEnumerable<UserData> reply = data.Select(e => e.Map());
        await responseStream.WriteAllAsync(reply);
    }

    public override async Task<UserData> GetById(Int32Value request, ServerCallContext context)
    {   
        Models.User data = await _repository.GetByIdAsync(request.Value, context.CancellationToken);
        return data.Map();
    }

    public override async Task<Empty> Create(UserData request, ServerCallContext context)
    {
        await _repository.CreateAsync(request.Map(), context.CancellationToken);
        return new Empty();
    }

    public override async Task<Empty> Update(UserData request, ServerCallContext context)
    {
        await _repository.UpdateAsync(request.Map(), context.CancellationToken);
        return new Empty();
    }

    public override async Task<Empty> Delete(Int32Value request, ServerCallContext context)
    {
        await  _repository.DeleteAsync(request.Value, context.CancellationToken);
        return new Empty();
    }
}