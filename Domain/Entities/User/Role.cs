using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.User;

public class Role : IdentityUserRole<int>
{
	public User User { get; set; }
	public UserRole UserRole { get; set; }
}