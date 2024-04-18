using Application.DTO.User;
using Application.Helper;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Persistence.Helper;
using Shared.Controllers;

namespace IdentityServiceApi.Controllers;

public class AccountController(IAccountRepository _accountRepository, IMapper _mapper) : BaseApiController
{
	[HttpPost, Route("Login")]
	public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto request)
	{
		var response = await _accountRepository.AccountLogin(request);

		if (response.IsT0)
		{
			if (response.AsT0.Item1.Succeeded && response.AsT0.Item2 != null)
			{
				return Ok(response.AsT0.Item2);
			}
			else
			{
				return BadRequest(response.AsT0.Item1);
			}
		}
		else
		{
			return BadRequest(response.AsT1.Value);
		}
	}

	[Authorize]
	[HttpPost, Route("Logout")]
	public async Task<ActionResult> Logout()
	{
		await _accountRepository.Logout();
		return Ok();
	}

	[HttpPost, Route("Register")]
	public async Task<ActionResult<RegisterResponseDTO>> Register(RegisterRequestDto registerDto)
	{
		var response = await _accountRepository.AccountRegister(registerDto);
		if (response.IsT0)
		{
			var userReturn = _mapper.Map<RegisterResponseDTO>(response.AsT0.Item3);
			userReturn.RoleCreationSuccess = response.AsT0.Item1.Succeeded;
			userReturn.RoleCreationError = response.AsT0.Item1.Errors?.ToString();
			return Ok(userReturn);
		}
		else
		{
			return BadRequest(response.AsT1.Value);
		}
	}

	[HttpGet, Route("Fetch")]
	public async Task<ActionResult<User>> Fetch(string email)
	{
		var response = await _accountRepository.Fetch(email);

		if (response.IsT0)
		{
			return response.AsT0;
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}

	[HttpGet, Route("Fetches")]
	public async Task<ActionResult<PagedList<User>>> Fetch([FromQuery] FilterParams filterParams)
	{
		var response = await _accountRepository.Fetch(filterParams);
		if (response.IsT0)
		{
			Response.AddPaginationHeader(response.AsT0.CurrentPage, response.AsT0.PageSize, response.AsT0.TotalCount, response.AsT0.TotalPages);
			return response.AsT0;
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}

	[HttpPut, Route("Update")]
	public async Task<ActionResult<bool>> Update(UpdateUserRequestDto request)
	{
		var response = await _accountRepository.Update(request);
		if (response.IsT0)
		{
			return Ok(response.AsT0);
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}

	[HttpDelete, Route("Delete")]
	public async Task<ActionResult<bool>> Delete(string email)
	{
		var response = await _accountRepository.Delete(email);

		if (response.IsT0)
		{
			return Ok(response.AsT0);
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}

	[HttpPost, Route("AccountRole_Add")]
	public async Task<ActionResult<IdentityResult>> AccountRole_Add(string email, string role)
	{
		var response = await _accountRepository.AccountRole_Add(email, role);
		if (response.IsT0)
		{
			return Ok(response.AsT0);
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}

	[HttpPost, Route("AccountRole_Remove")]
	public async Task<ActionResult<IdentityResult>> AccountRole_Remove(string email, string role)
	{
		var response = await _accountRepository.AccountRole_Remove(email, role);
		if (response.IsT0)
		{
			return Ok(response.AsT0);
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}

	[HttpPost, Route("AccountPhone_CreateToken")]
	public async Task<ActionResult<string>> AccountPhone_CreateToken(string phoneNumber)
	{
		var response = await _accountRepository.AccountPhone_CreateToken(phoneNumber);
		if (response.IsT0)
		{
			return Ok(response.AsT0);
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}

	[HttpPost, Route("AccountPhone_Change")]
	public async Task<ActionResult<bool>> AccountPhone_Change(string token, string newPhoneNumber)
	{
		var response = await _accountRepository.AccountPhone_Change(token, newPhoneNumber);
		if (response.IsT0)
		{
			return Ok(response.AsT0);
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}

	[HttpPost, Route("AccountPhone_Confirmed")]
	public async Task<ActionResult<bool>> AccountPhone_Confirmed()
	{
		var response = await _accountRepository.AccountPhone_Confirmed();
		if (response.IsT0)
		{
			return Ok(response.AsT0);
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}

	[HttpPost, Route("AccountPassword_Change")]
	public async Task<ActionResult<bool>> AccountPassword_Change(string newPassword, string token)
	{
		var response = await _accountRepository.AccountPassword_Change(newPassword, token);
		if (response.IsT0)
		{
			return Ok(response.AsT0);
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}

	[HttpPost, Route("AccountPassword_CreateToken")]
	public async Task<ActionResult<string>> AccountPassword_CreateToken()
	{
		var response = await _accountRepository.AccountPassword_CreateToken();
		if (response.IsT0)
		{
			return Ok(response.AsT0);
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}

	[HttpPost, Route("TwoFactorAuthSetup")]
	public async Task<ActionResult<TfaSetupDto>> TwoFactorAuthSetup(string email)
	{
		var response = await _accountRepository.TwoFactorAuthSetup(email);
		if (response.IsT0)
		{
			return Ok(response.AsT0);
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}

	[HttpPost, Route("PostTfaSetup")]
	public async Task<ActionResult<TfaSetupDto>> PostTfaSetup(TfaSetupDto tfaModel)
	{
		var response = await _accountRepository.PostTfaSetup(tfaModel);
		if (response.IsT0)
		{
			return Ok(response.AsT0);
		}
		else
		{
			return BadRequest(response.AsT1);
		}
	}
}