using Domain.Entities;
using Domain.Entities.User;
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
	}

	public DbSet<User> db_User { get; set; }
	public DbSet<UserMetaData> db_UserMetaData { get; set; }
	public DbSet<LogEntry> LogEntries { get; set; }
	public DbSet<Message> db_Messages { get; set; }
}