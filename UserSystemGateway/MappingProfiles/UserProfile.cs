using Google.Protobuf.WellKnownTypes;
using SimpleUserManagementSystem.Common.Protos;

namespace UserSystemGateway.MappingProfiles;

public static class UserProfile
{
    public static UserData Map(this Models.User model)
    {
        return new UserData
        {
            Id = model.ID,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            MiddleName = model.MiddleName,
            Birthday = Timestamp.FromDateTime(DateTime.SpecifyKind(model.Birthday, DateTimeKind.Utc))
        };
    }
}