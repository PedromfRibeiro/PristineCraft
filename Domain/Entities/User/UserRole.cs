using Microsoft.AspNetCore.Identity;

namespace PristineCraft.Domain.Entities.User;

public class UserRole : IdentityRole<int>
{
    public ICollection<AppRole>? Role { get; set; }
}