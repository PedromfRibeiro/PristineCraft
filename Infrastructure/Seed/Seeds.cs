using PristineCraft.Domain.Entities.User;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PristineCraft.Domain.Entities.Payee;
using PristineCraft.Domain.Entities;

namespace Infrastructure.Helper;

public static class Seeds
{
    public static async Task SeederStartUpVerification(UserManager<AppUser> userManager, DataContext _context, RoleManager<UserRole> roleManager)
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        if (_context.Database.GetPendingMigrations().Any())
        {
            try
            {
                _context.Database.EnsureDeleted();
                _context.Database.Migrate();
                await CreateCategories(_context);
                await CreateBankAccount(_context);
                await UserSeedAsync(userManager, _context, roleManager);
            }
            catch
            {
                Console.WriteLine("An error occurred durring migration");
            }
        }
    }

    private static async Task UserSeedAsync(UserManager<AppUser> userManager, DataContext _context, RoleManager<UserRole> roleManager)
    {
        try
        {
            await SeedUser.CreateRoles(roleManager);
            await _context.SaveChangesAsync();
            List<AppUser> listUser = await SeedUser.Seeder(userManager);
            await _context.DbUser.AddRangeAsync(listUser);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }

    private static async Task CreateCategories(DataContext _context)
    {
        var jsonData = File.ReadAllText("../Infrastructure/Seed/Resources/payee.json");
        var storeCategories = JsonConvert.DeserializeObject<Dictionary<string, List<Payee>>>(jsonData);
        if (storeCategories == null)
            return;

        foreach (var item in storeCategories.Keys)
        {
            List<Payee> ListPayee = storeCategories[item];
            var category = _context.DbPayeeCategory.Where(x => x.Name == item).FirstOrDefault();
            if (category == null)
            {
                category = new PayeeCategory() { Name = item };
            }
            foreach (Payee payee in ListPayee)
            {
                payee.Category = category;
            }
            await _context.DbPayee.AddRangeAsync(ListPayee);
        }
        _context.SaveChanges();
    }

    private static async Task CreateBankAccount(DataContext _context)
    {
        var jsonData = File.ReadAllText("../Infrastructure/Seed/Resources/BankAccount.json");
        var items = JsonConvert.DeserializeObject<List<BankAccount>>(jsonData);
        if (items == null)
            return;

        foreach (var item in items)
        {
            Random rand = new Random();
            int toSkip = rand.Next(0, _context.DbUser.Count());
            item.Proprietary = _context.DbUser.OrderBy(r => r.Id).Skip(toSkip).Take(1).FirstOrDefault();
            await _context.DbBankAccount.AddAsync(item);
        }
        _context.SaveChanges();
    }
}