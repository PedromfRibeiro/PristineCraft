using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PristineCraft.Application.Common.Interfaces;
using PristineCraft.Domain.Entities.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PristineCraft.Application.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<AppUser> _userManager;

    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration config, UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        //TODO: Empty Validation
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"] ?? "asd"));
    }

    public async Task<string> CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            //TODO: Empty validation
            new Claim(JwtRegisteredClaimNames.NameId, user.UserName??"Not Found"),
            new Claim(JwtRegisteredClaimNames.Email , user.Email??"Not Found")
        };
        claims.AddRange((await _userManager.GetRolesAsync(user)).Select(role => new Claim(ClaimTypes.Role, role)));
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        var token = new JwtSecurityTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature)
        });

        return tokenHandler.WriteToken(token);
    }
}