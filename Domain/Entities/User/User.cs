using Bogus;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Domain.Entities.User;

public class User : IdentityUser<int>
{
	public string Name { get; set; }
	public string? Contact { get; set; }
	public byte[]? Image { get; set; }
	public byte[]? ImageSmall { get; set; }
	public string? Observations { get; set; }
	public EnumGender Gender { get; set; }

	#region Alternate keys

	public ICollection<UserMetaData> UserMetaData { get; set; }
	public ICollection<Role> UserRole { get; set; }

	#endregion Alternate keys

	#region Seeder

	public static async Task<List<User>> Seeder(UserManager<User> userManager)
	{
		try
		{
			Randomizer.Seed = new Random(8675309);
			Faker<User> FakeUsers = new Faker<User>(locale: "pt_PT")
				.StrictMode(false)
				.RuleFor(o => o.Id, f => 0)
				.RuleFor(o => o.Name, f => f.Person.FullName)
				.RuleFor(o => o.Contact, f => f.Person.Phone)
				.RuleFor(o => o.Image, f => DownloadImageFromUrl(f.Person.Avatar))
				.RuleFor(o => o.ImageSmall, f => DownloadImageFromUrl(f.Person.Avatar))
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

	public static async Task CreateRoles(RoleManager<UserRole> roleManager)
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

	private static byte[] DownloadImageFromUrl(string url)
	{
		byte[] imageBytes;
		using (WebClient webClient = new())
		{
			imageBytes = webClient.DownloadData(url);
		}
		return imageBytes;
	}
	#endregion Seeder
}