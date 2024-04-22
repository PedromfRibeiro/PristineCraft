using PristineCraft.Domain.Entities.User;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

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
                await UserSeedAsync(userManager, _context, roleManager);
                Seeder.CreateCategories(_context);
                Seeder.CreateBankAccount(_context);
            }
            catch
            {
                throw new Exception("An error occurred durring migration");
            }
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
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
}