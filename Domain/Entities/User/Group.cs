namespace Domain.Entities;

public class Group
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public ICollection<User>? Relatives { get; set; } = new List<User>();
	public required User Owner { get; set; }
}