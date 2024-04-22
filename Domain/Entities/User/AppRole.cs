namespace PristineCraft.Domain.Entities.User;

public class AppRole : IdentityUserRole<int>
{
    public AppUser? User { get; set; }
    public UserRole? UserRole { get; set; }
}