using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.User;

public class UserRole : IdentityRole<int>
{
	public ICollection<Role> Role { get; set; }
}