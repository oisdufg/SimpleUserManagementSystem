using System.ComponentModel.DataAnnotations;

namespace UserSystemGateway.Models.DTO;

public class UpdateUserTaskRequest
{
    private const int MaxTaskNameLength = 255;
    private const int MaxTaskDescriptionLength = 350;
    
    [Range(1, Int32.MaxValue, ErrorMessage = "Parameter '{0}' must be greater than 0")]
    public int ID { get; set; }
    
    [Range(1, Int32.MaxValue, ErrorMessage = "Parameter '{0}' must be greater than 0")]
    public int? UserID { get; set; }
    
    [StringLength(MaxTaskNameLength, ErrorMessage = "Parameter '{0}' has invalid length. Maximum property length - {1} symbols", MinimumLength = 1)]
    public string? TaskName { get; set; }
    
    [StringLength(MaxTaskDescriptionLength, ErrorMessage = "Parameter '{0}' has invalid length. Maximum property length - {1} symbols", MinimumLength = 1)]
    public string? TaskDescription { get; set; }
    
    [Required(ErrorMessage = "Parameter '{0}' cannot be empty")]
    public byte[] RowVersion { get; set; }
}