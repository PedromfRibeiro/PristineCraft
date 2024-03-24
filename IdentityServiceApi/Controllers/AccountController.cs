using Application.DTO.User;
using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using OneOf.Types;
using OneOf;
using Shared.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServiceApi.Controllers;

public class AccountController(IAccountRepository _accountRepository, ITokenService _tokenService, IMapper _mapper) : BaseApiController
{
	[HttpPost, Route("Login")]
	public async Task<ActionResult<OneOf<(SignInResult, LoginResponseDto), Error<string>>>> Login(LoginRequestDto request)
	{
		var response = await _accountRepository.AccountLogin(request);
		return response.Match(
							user => Ok(user.ToTuple()),
							error => Ok(error)
							);
	}

	[Authorize]
	[HttpPost, Route("Logout")]
	public async Task<ActionResult> Logout()
	{
		await _accountRepository.Logout();
		return Ok();
	}
}