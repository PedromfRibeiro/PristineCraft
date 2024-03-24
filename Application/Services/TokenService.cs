using Application.Interfaces;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services;

public class TokenService : ITokenService
{
	private readonly UserManager<User> _userManager;

	private readonly SymmetricSecurityKey _key;

	public TokenService(IConfiguration config, UserManager<User> userManager)
	{
		_userManager = userManager;

		_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
	}

	public async Task<string> CreateToken(User user)
	{
		var claims = new List<Claim>
		{
			new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
			new Claim(JwtRegisteredClaimNames.Email , user.Email)
		};
		claims.AddRange((await _userManager.GetRolesAsync(user)).Select(role => new Claim(ClaimTypes.Role, role)));
		JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
		var token = (new JwtSecurityTokenHandler()).CreateToken(new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.Now.AddDays(7),
			SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature)
		});

		return tokenHandler.WriteToken(token);
	}
}