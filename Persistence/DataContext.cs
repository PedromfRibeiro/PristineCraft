using Domain.Entities;
using Domain.Entities.Event;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DataContext : IdentityDbContext<User, UserRole, int, IdentityUserClaim<int>, Role, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
	public DataContext(DbContextOptions<DataContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<User>()
				 .HasMany(ur => ur.UserRole)
				 .WithOne(u => u.User)
				 .HasForeignKey(ur => ur.UserId)
				 .IsRequired();

		builder.Entity<UserRole>()
				 .HasMany(ur => ur.Role)
				 .WithOne(u => u.UserRole)
				 .HasForeignKey(ur => ur.RoleId)
				 .IsRequired()
				 .OnDelete(DeleteBehavior.NoAction);

		builder.Entity<Message>()
	   .HasOne(m => m.Sender)
	   .WithMany(u => u.SentMessages)
	   .HasForeignKey(m => m.SenderId)
	   .IsRequired()
	   .OnDelete(DeleteBehavior.NoAction); // Specify OnDelete behavior

		builder.Entity<Message>()
			.HasOne(m => m.Receiver)
			.WithMany(u => u.ReceivedMessages)
			.HasForeignKey(m => m.ReceiverId)
			.IsRequired()
			.OnDelete(DeleteBehavior.NoAction); // Specify OnDelete behavior
	}

	public DbSet<User> DbUser { get; set; }
	public DbSet<UserMetaData> DbUserMetaData { get; set; }
	public DbSet<LogEntry> LogEntries { get; set; }
	public DbSet<Message> DbMessages { get; set; }
	public DbSet<Payee> DbPayee { get; set; }
	public DbSet<PayeeCategory> DbPayeeCategory { get; set; }
	public DbSet<BankAccount> DbBankAccount { get; set; }
	public DbSet<Event> DbEvent { get; set; }
	public DbSet<EventCategory> DbEventCategory { get; set; }
	public DbSet<EventSubCategory> DbEventSubCategory { get; set; }
	public DbSet<Group> DbGroup { get; set; }
}