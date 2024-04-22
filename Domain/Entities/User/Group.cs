using System.ComponentModel.DataAnnotations;

namespace PristineCraft.Domain.Entities.User;

public class Group
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public ICollection<User>? Relatives { get; set; } = new List<User>();
	public required User Owner { get; set; }

    public Group(AppUser owner, string name)
    {
        Owner = owner;
        Name = name;
        Relatives.Add(owner); // Include owner as the first member of the group
    }
}