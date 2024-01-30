using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DataSeeding;
public static class Seeds
{
	public static async Task SeederStartUpVerification(UserManager<User> userManager, DataContext _context, RoleManager<UserRole> roleManager)
	{
		var checkDatabaseContent = await _context.db_User.CountAsync();

		if (checkDatabaseContent > 0)
			await UserSeedAsync(userManager, _context, roleManager);

	}
	public static async Task UserSeedAsync(UserManager<User> userManager, DataContext _context, RoleManager<UserRole> roleManager)
	{
		try
		{
			await User.CreateRoles(roleManager);
			await _context.SaveChangesAsync();

			List<User> listUser = await User.Seeder(userManager);
			await _context.db_User.AddRangeAsync(listUser);
		}
		catch (Exception ex)
		{
			Console.Write(ex.ToString());
		}



		await _context.SaveChangesAsync();
	}
}
