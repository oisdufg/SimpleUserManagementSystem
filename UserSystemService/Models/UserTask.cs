using System.ComponentModel.DataAnnotations;
using TaskStatus = UserSystemService.Enums.TaskStatus;

namespace UserSystemService.Models;

public class UserTask
{
    public int ID { get; set; }
    public int? UserID { get; set; }
    public string TaskName { get; set; }
    public string? TaskDescription { get; set; }
    public TaskStatus TaskStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public bool IsDeleted  { get; set; }
    
    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;
    
    public User? User { get; set; }

    public override int GetHashCode()
    {
        return HashCode.Combine(ID, UserID, TaskName, TaskDescription, TaskStatus, CreatedAt, ModifiedAt, IsDeleted);
    }

    public override bool Equals(object obj)
    {
        return obj is UserTask toCompare
               && ID == toCompare.ID
               && UserID == toCompare.UserID
               && TaskName == toCompare.TaskName
               && TaskDescription == toCompare.TaskDescription
               && TaskStatus == toCompare.TaskStatus
               && CreatedAt == toCompare.CreatedAt
               && ModifiedAt == toCompare.ModifiedAt
               && IsDeleted == toCompare.IsDeleted;
    }
}