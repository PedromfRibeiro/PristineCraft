namespace Application.DTO.User;

public class GroupDto
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public ICollection<UserDto>? Relatives { get; set; } = new List<UserDto>();
	public required UserDto Owner { get; set; }
}