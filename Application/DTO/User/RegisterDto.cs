using System.ComponentModel.DataAnnotations;

namespace Application.DTO.User;

public class RegisterRequestDto
{
	[Required]
	public string Name { get; set; }
	public string Contact { get; set; }
	public string PhoneNumber { get; set; }
	public string Observations { get; set; }


	//IdentityUser Fields
	[Required]
	public string UserName { get; set; }
	[Required]
	public string Email { get; set; }
	[Required]
	[StringLength(20, MinimumLength = 4)]
	public string Password { get; set; }
}
public class RegisterResponseDTO
{
	public string UserName { get; set; }
	public string Email { get; set; }
}
