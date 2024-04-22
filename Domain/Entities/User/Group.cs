using System.ComponentModel.DataAnnotations;

namespace PristineCraft.Domain.Entities.User;

public class Group
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<AppUser>? Relatives { get; set; } = new List<AppUser>();
    public required AppUser Owner { get; set; }

    public Group(AppUser owner, string name)
    {
        Owner = owner;
        Name = name;
        Relatives.Add(owner); // Include owner as the first member of the group
    }
}