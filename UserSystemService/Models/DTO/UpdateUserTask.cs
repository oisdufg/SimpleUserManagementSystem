namespace UserSystemService.Models;

public class UpdateUserTask
{
    public int ID { get; set; }
    public int? UserID { get; set; }
    public string? TaskName { get; set; }
    public string? TaskDescription { get; set; }
    public byte[] RowVersion { get; set; }
}