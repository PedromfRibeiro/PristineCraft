using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class UserRole : IdentityRole<int>
{
	public ICollection<Role> Role { get; set; }
}