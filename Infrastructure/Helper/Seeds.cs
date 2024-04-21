using PristineCraft.Domain.Entities.User;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Helper;

public static class Seeds
{
    public static async Task SeederStartUpVerification(UserManager<AppUser> userManager, DataContext _context, RoleManager<UserRole> roleManager)
    {
        var checkDatabaseContent = await _context.DbUser.CountAsync();
        if (checkDatabaseContent == 0)
        {
            await UserSeedAsync(userManager, _context, roleManager);
            await Seeder.CreateCategories(_context);
            await Seeder.CreateBankAccount(_context);
        }
    }

    public static async Task UserSeedAsync(UserManager<AppUser> userManager, DataContext _context, RoleManager<UserRole> roleManager)
    {
        try
        {
            await SeedUser.CreateRoles(roleManager);
            await _context.SaveChangesAsync();
            List<AppUser> listUser = await SeedUser.Seeder(userManager);
            await _context.DbUser.AddRangeAsync(listUser);
        }
        catch (Exception ex)
        {
            Console.Write(ex.ToString());
        }
        await _context.SaveChangesAsync();
    }
}