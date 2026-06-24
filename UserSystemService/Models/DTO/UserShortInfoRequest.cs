namespace UserSystemService.Models.DTO;

public class UserShortInfoRequest
{
    public int ID { get; set; }
    public string FullName { get; set; }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(ID, FullName);
    }

    public override bool Equals(object obj)
    {
        return obj is UserShortInfoRequest toCompare
            && ID == toCompare.ID
            && FullName == toCompare.FullName;
    }
}