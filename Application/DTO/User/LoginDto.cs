using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.User;
public class LoginRequestDto
{
	[Required]
	public required string Email { get; set; }

	[Required]
	public required string Password { get; set; }

	public bool RememberLogin { get; set; }
	public required string ReturnUrl { get; set; }
}

public class LoginResponseDto
{
	public required string Name { get; set; }
	public required string Email { get; set; }
	public bool EmailConfirmed { get; set; }
	public required string NormalizedEmail { get; set; }
	public required string UserName { get; set; }
	public required string NormalizedUserName { get; set; }
	public required string Contact { get; set; }
	public required byte[] ImageSmall { get; set; }
	public required string Observations { get; set; }
	public required string Token { get; set; }
	public int CompanyId { get; set; }
	public bool TwoFactorEnabled { get; set; }
	public EnumGender Gender { get; set; }
}