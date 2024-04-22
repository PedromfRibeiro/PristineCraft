using PristineCraft.Domain.Entities.User;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Extensions;

public static class IdentityExtensions
{
    public static IServiceCollection CreateIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = true;
            opt.User.RequireUniqueEmail = true;
        })
        .AddRoles<UserRole>()
        .AddRoleManager<RoleManager<UserRole>>()
        .AddRoleValidator<RoleValidator<UserRole>>()
        .AddSignInManager<SignInManager<AppUser>>()
        .AddEntityFrameworkStores<DataContext>();

        services.AddAuthentication(option =>
        {
            option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer("Bearer", options =>
                  {
                      options.IncludeErrorDetails = true;
                      options.TokenValidationParameters = new TokenValidationParameters
                      {
                          ValidateIssuerSigningKey = true,
                          //TODO: Null Check
                          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"] ?? "")),
                          ValidateIssuer = false,
                          ValidateAudience = false,
                      };
                  });

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("Admin", policy => policy.RequireRole("Admin", "Moderator"));
            opt.AddPolicy("Moderator", policy => policy.RequireRole("CompanyOwner"));
            opt.AddPolicy("CompanyOwner", policy => policy.RequireRole("CompanyMember"));
            opt.AddPolicy("CompanyMember", policy => policy.RequireRole("MemberBasic"));
            opt.AddPolicy("MemberPremium", policy => policy.RequireRole("MemberBasic"));
            opt.AddPolicy("MemberBasic", policy => policy.RequireRole("MemberBasic"));
        });

        services.AddDbContext<DataContext>(
            opt => opt.UseSqlServer(
                configuration.GetConnectionString("SQLServerCon"),
                x => x.MigrationsAssembly("PristineCraft.Infrastructure")
            ));
        services.AddSignalR();
        return services;
    }

    public static string GenerateIssuerSigningKey()
    {
        var rsa = RSA.Create();
        return Convert.ToBase64String(rsa.ExportRSAPrivateKey());
    }
}