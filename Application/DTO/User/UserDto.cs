using Domain.Entities;

namespace Application.DTO.User;

public class UserDto
{
	public virtual Guid Id { get; set; }
	public required string Name { get; set; }
	public required string Email { get; set; }
	public bool EmailConfirmed { get; set; }
	public required string NormalizedEmail { get; set; }
	public required string UserName { get; set; }
	public required string NormalizedUserName { get; set; }
	public required string Contact { get; set; }
	public required byte[] ImageSmall { get; set; }
	public required string Observations { get; set; }
	public bool TwoFactorEnabled { get; set; }
	public EnumGender Gender { get; set; }
}