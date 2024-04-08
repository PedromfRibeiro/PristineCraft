namespace Domain.Entities;

public class Group
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public ICollection<User>? Relatives { get; set; } = new List<User>();
	public required User Owner { get; set; }

	public Group(User owner, string name)
	{
		Owner = owner;
		Name = name;
		Relatives.Add(owner); // Include owner as the first member of the group
	}
}