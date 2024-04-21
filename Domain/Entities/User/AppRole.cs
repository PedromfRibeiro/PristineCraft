namespace PristineCraft.Domain.Entities.User;

public class AppRole : IdentityUserRole<int>
{
    public required AppUser User { get; set; }
    public required UserRole UserRole { get; set; }
}