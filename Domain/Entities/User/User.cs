using Bogus;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Domain.Entities;

public class User : IdentityUser<int>
{
	public required string Name { get; set; }
	public string? Contact { get; set; }
	public byte[]? Image { get; set; }
	public byte[]? ImageSmall { get; set; }
	public string? Observations { get; set; }
	public EnumGender Gender { get; set; }
	public ICollection<UserMetaData> UserMetaData { get; set; } = new List<UserMetaData>();
	public ICollection<Message> Messages { get; set; } = new List<Message>();
	public ICollection<Role> UserRole { get; set; }
	public ICollection<BankAccount>? BankAccounts { get; set; }

	// Navigation property for messages sent by this user
	public virtual ICollection<Message> SentMessages { get; set; }

	// Navigation property for messages received by this user
	public virtual ICollection<Message> ReceivedMessages { get; set; }
}