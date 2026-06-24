using System.ComponentModel.DataAnnotations;

namespace UserSystemGateway.Models.DTO;

public class CreateUserTaskRequest
{
    private const int MaxTaskNameLength = 255;
    private const int MaxTaskDescriptionLength = 350;
        
    [Range(1, int.MaxValue, ErrorMessage = "Parameter '{0}' must be greater than 0")]
    public int UserID { get; set; }

    [Required(ErrorMessage = "Parameter '{0}' cannot be empty")]
    [StringLength(MaxTaskNameLength,
        ErrorMessage = "Parameter '{0}' has invalid length. Maximum property length - {1} symbols", MinimumLength = 1)]
    public string TaskName { get; set; } = null!;
    
    [StringLength(MaxTaskDescriptionLength, ErrorMessage = "Parameter '{0}' has invalid length. Maximum property length - {1} symbols")]
    public string? TaskDescription { get; set; }
}