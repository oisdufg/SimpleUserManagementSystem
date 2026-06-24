using TaskStatus = UserSystemGateway.Enums.TaskStatus;

namespace UserSystemGateway.Models;

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
    public byte[] RowVersion { get; set; } = null!;
    
    public User? User { get; set; }
}