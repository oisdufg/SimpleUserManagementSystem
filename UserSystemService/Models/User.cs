namespace UserSystemService.Models;

public class User
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Email { get; set; }
    public DateTime Birthday { get; set; }

    public ICollection<UserTask> Tasks { get; set; } = [];

    public override int GetHashCode()
    {
        return HashCode.Combine(ID, Email, FirstName, LastName, MiddleName, Birthday);
    }

    public override bool Equals(object obj)
    {
        return obj is User toCompare
               && ID == toCompare.ID
               && FirstName == toCompare.FirstName
               && LastName == toCompare.LastName
               && MiddleName == toCompare.MiddleName
               && Email == toCompare.Email
               && Birthday == toCompare.Birthday;
    }
}