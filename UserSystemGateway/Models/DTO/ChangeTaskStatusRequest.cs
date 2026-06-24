using System.ComponentModel.DataAnnotations;
using TaskStatus = UserSystemGateway.Enums.TaskStatus;

namespace UserSystemGateway.Models.DTO;

public class ChangeTaskStatusRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Parameter '{0}' must be greater than 0")]
    public int ID { get; set; }
    
    [EnumDataType(typeof(TaskStatus), ErrorMessage = "Parameter '{0}' has invalid value")]
    public TaskStatus TaskStatus { get; set; }
}