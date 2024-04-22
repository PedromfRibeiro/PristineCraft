using Bogus;
using PristineCraft.Domain.Entities;
using PristineCraft.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.Seed;

internal class SeedUser
{
    internal static async Task<List<AppUser>> Seeder(UserManager<AppUser> userManager)
    {
        try
        {
            Randomizer.Seed = new Random(8675309);
            Faker<AppUser> FakeUsers = new Faker<AppUser>(locale: "pt_PT")
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

            List<AppUser> users = FakeUsers.Generate(20);
            foreach (AppUser user in users)
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
                new UserRole { Id=0, Name = "Admin" },
                new UserRole { Id=0, Name = "Moderator" },
                new UserRole { Id=0, Name = "CompanyOwner" },
                new UserRole { Id=0, Name = "CompanyMember" },
                new UserRole { Id=0, Name = "MemberPremium" },
                new UserRole { Id=0, Name = "MemberBasic" },
            ];
        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }
    }

    private static byte[] DownloadImageFromUrl(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    byte[] imageBytes = response.Content.ReadAsByteArrayAsync().Result;
                    return imageBytes;
                }
                else
                {
                    Console.WriteLine($"Failed to download image. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return new byte[0];
        }
    }
}