using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using SimpleUserManagementSystem.Common.Protos;
using TaskStatus = SimpleUserManagementSystem.Common.Protos.TaskStatus;

namespace UserSystemGateway.MappingProfiles;

public static class UserTaskProfile
{
    public static CreateUserTaskData Map(this Models.DTO.CreateUserTaskRequest model)
    {
        return new CreateUserTaskData
        {
            UserId = model.UserID,
            TaskName = model.TaskName,
            TaskDescription = model.TaskDescription,
        };
    }

    public static UpdateUserTaskData Map(this Models.DTO.UpdateUserTaskRequest model)
    {
        return new UpdateUserTaskData
        {
            Id = model.ID,
            UserId = model.UserID,
            TaskName = model.TaskName,
            TaskDescription = model.TaskDescription,
        };
    }

    public static ChangeStatusData Map(this Models.DTO.ChangeTaskStatusRequest model)
    {
        return new ChangeStatusData
        {
            Id = model.ID,
            TaskStatus = (TaskStatus)model.TaskStatus
        };
    }
}