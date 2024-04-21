namespace PristineCraft.Domain.Entities.User;

public class AppUser : IdentityUser<int>
{
    public required string Name { get; set; }
    public string? Contact { get; set; }
    public byte[]? Image { get; set; }
    public byte[]? ImageSmall { get; set; }
    public string? Observations { get; set; }
    public EnumGender Gender { get; set; }
    public ICollection<UserMetaData> UserMetaData { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
    public required ICollection<AppRole> UserRole { get; set; }
    public ICollection<BankAccount>? BankAccounts { get; set; }

    // Navigation property for messages sent by this user
    public virtual ICollection<Message>? SentMessages { get; set; }

    // Navigation property for messages received by this user
    public virtual ICollection<Message>? ReceivedMessages { get; set; }
}