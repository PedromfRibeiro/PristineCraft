using PristineCraft.Domain.Entities.User;
using PristineCraft.Domain.Entities.Payee;
using PristineCraft.Domain.Entities;

namespace PristineCraft.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<AppUser> DbUser { get; }
    DbSet<UserMetaData> DbUserMetaData { get; }
    DbSet<LogEntry> LogEntries { get; }
    DbSet<Message> DbMessages { get; }
    DbSet<Payee> DbPayee { get; }
    DbSet<PayeeCategory> DbPayeeCategory { get; }
    DbSet<BankAccount> DbBankAccount { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}