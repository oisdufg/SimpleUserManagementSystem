using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using SimpleUserManagementSystem.Common.Protos;

namespace UserSystemService.MappingProfiles;

public static class UserTaskProfile
{
    public static UserTaskData Map(this Models.UserTask model)
    {
        return new UserTaskData
        {
            Id = model.ID,
            UserId = model.UserID,
            TaskName = model.TaskName,
            TaskDescription = model.TaskDescription,
            TaskStatus = (SimpleUserManagementSystem.Common.Protos.TaskStatus)model.TaskStatus,
            CreatedAt = Timestamp.FromDateTime(DateTime.SpecifyKind(model.CreatedAt, DateTimeKind.Utc)),
            ModifiedAt = Timestamp.FromDateTime(DateTime.SpecifyKind(model.ModifiedAt, DateTimeKind.Utc)),
            IsDeleted = model.IsDeleted,
            RowVersion = ByteString.CopyFrom(model.RowVersion)
        };
    }

    public static Models.UserTask Map(this CreateUserTaskData model)
    {
        return new Models.UserTask
        {
            UserID = model.UserId,
            TaskName = model.TaskName,
            TaskDescription = model.TaskDescription
        };
    }
    public static Models.UpdateUserTaskRequest Map(this UpdateUserTaskData model)
    {
        return new Models.UpdateUserTaskRequest
        {
            ID = model.Id,
            UserID = model.UserId,
            TaskName = model.TaskName,
            TaskDescription = model.TaskDescription,
        };
    }  
}