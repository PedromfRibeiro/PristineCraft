using Bogus;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Seed;

internal class SeedUser
{
	internal static async Task<List<User>> Seeder(UserManager<User> userManager)
	{
		try
		{
			Randomizer.Seed = new Random(8675309);
			Faker<User> FakeUsers = new Faker<User>(locale: "pt_PT")
				.StrictMode(false)
				.RuleFor(o => o.Id, f => 0)
				.RuleFor(o => o.Name, f => f.Person.FullName)
				.RuleFor(o => o.Contact, f => f.Person.Phone)
				.RuleFor(o => o.Image, f => Utils.DownloadImageFromUrl(f.Person.Avatar))
				.RuleFor(o => o.ImageSmall, f => Utils.DownloadImageFromUrl(f.Person.Avatar))
				.RuleFor(o => o.Observations, f => f.Random.Word())
				.RuleFor(o => o.Gender, f => f.PickRandom<EnumGender>())
				.RuleFor(o => o.Email, f => f.Person.Email)
				.RuleFor(o => o.UserName, f => f.Person.UserName);

			List<User> users = FakeUsers.Generate(20);
			foreach (User user in users)
			{
				await userManager.CreateAsync(user, "Pa$$w0rd");
				await userManager.AddToRoleAsync(user, "MemberBasic");
			}
			return users;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.ToString());
		}
	}

	internal static async Task CreateRoles(RoleManager<UserRole> roleManager)
	{
		List<UserRole> roles =
			[
				new UserRole { Name = "Admin" },
				new UserRole { Name = "Moderator" },
				new UserRole { Name = "CompanyOwner" },
				new UserRole { Name = "CompanyMember" },
				new UserRole { Name = "MemberPremium" },
				new UserRole { Name = "MemberBasic" },
			];
		foreach (var role in roles)
		{
			await roleManager.CreateAsync(role);
		}
	}
}