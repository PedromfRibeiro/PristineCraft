using System.ComponentModel.DataAnnotations;

namespace PristineCraft.Domain.Entities.User;

public class UserMetaData
{
    [Key]
    public int Id { get; set; }
    public string? Parameter { get; set; }
    public string? Value { get; set; }
    public bool Valid { get; set; } = true;
    public int UserId { get; set; }
    public required AppUser User { get; set; }
}