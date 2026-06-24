using Google.Protobuf.WellKnownTypes;
using SimpleUserManagementSystem.Common.Protos;

namespace UserSystemService.MappingProfiles;

public static class UserProfile
{
    public static Models.User Map(this UserData model)
    {
        return new Models.User
        {
            ID = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            MiddleName = model.MiddleName,
            Email = model.Email,
            Birthday = model.Birthday.ToDateTime(),
        };
    }
    
    public static UserData Map(this Models.User model)
    {
        return new UserData
        {
            Id = model.ID,
            FirstName = model.FirstName,
            LastName = model.LastName,
            MiddleName = model.MiddleName,
            Email = model.Email,
            Birthday = Timestamp.FromDateTime(DateTime.SpecifyKind(model.Birthday, DateTimeKind.Utc)),
        };
    }
}