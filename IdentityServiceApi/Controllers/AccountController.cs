using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Helper;
using Infrastructure.Controller;
using PristineCraft.Domain.Entities.User;
using PristineCraft.Application.Common.Interfaces;
using PristineCraft.Application.DTO.User;
using PristineCraft.Application.Common.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Azure;

namespace IdentityServiceApi.Controllers;

public class AccountController(IAccountRepository _accountRepository) : BaseApiController
{
    [HttpPost, Route("Login")]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto request)
    {
        var (signInResponse, loginDto) = await _accountRepository.AccountLogin(request);
        if (signInResponse.Succeeded && loginDto != null)
        {
            return Ok(loginDto);
        }
        else
        {
            return BadRequest(signInResponse);
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
        var (RoleResponse, UserResponse, RegisteDTO) = await _accountRepository.AccountRegister(registerDto);
        if (RoleResponse.Succeeded && UserResponse.Succeeded && UserResponse != null)
        {
            return Ok(RegisteDTO);
        }
        else
        {
            return BadRequest(UserResponse);
        }
    }

    //[HttpPost, Route("Fetch")]
    //public async Task<ActionResult<AppUser>> Fetch(string email)
    //{
    //    var response = await _accountRepository.Fetch(email);

    //    if (response.IsT0)
    //    {
    //        return response.AsT0;
    //    }
    //    else
    //    {
    //        return BadRequest(response.AsT1);
    //    }
    //}

    //[HttpPost, Route("Fetches")]
    //public async Task<ActionResult<PagedList<AppUser>>> Fetch(FilterParams filterParams)
    //{
    //    var response = await _accountRepository.Fetch(filterParams);
    //    if (response.IsT0)
    //    {
    //        Response.AddPaginationHeader(response.AsT0.CurrentPage, response.AsT0.PageSize, response.AsT0.TotalCount, response.AsT0.TotalPages);
    //        return response.AsT0;
    //    }
    //    else
    //    {
    //        return BadRequest(response.AsT1);
    //    }
    //}

    //[HttpPost, Route("Update")]
    //public async Task<ActionResult<bool>> Update(UpdateUserRequestDto request)
    //{
    //    var response = await _accountRepository.Update(request);
    //    if (response.IsT0)
    //    {
    //        return Ok(response.AsT0);
    //    }
    //    else
    //    {
    //        return BadRequest(response.AsT1);
    //    }
    //}

    //[HttpPost, Route("Delete")]
    //public async Task<ActionResult<bool>> Delete(string email)
    //{
    //    var response = await _accountRepository.Delete(email);

    //    if (response.IsT0)
    //    {
    //        return Ok(response.AsT0);
    //    }
    //    else
    //    {
    //        return BadRequest(response.AsT1);
    //    }
    //}

    //[HttpPost, Route("AccountRole_Add")]
    //public async Task<ActionResult<IdentityResult>> AccountRole_Add(string email, string role)
    //{
    //    var response = await _accountRepository.AccountRole_Add(email, role);
    //    if (response.IsT0)
    //    {
    //        return Ok(response.AsT0);
    //    }
    //    else
    //    {
    //        return BadRequest(response.AsT1);
    //    }
    //}

    //[HttpPost, Route("AccountRole_Remove")]
    //public async Task<ActionResult<IdentityResult>> AccountRole_Remove(string email, string role)
    //{
    //    var response = await _accountRepository.AccountRole_Remove(email, role);
    //    if (response.IsT0)
    //    {
    //        return Ok(response.AsT0);
    //    }
    //    else
    //    {
    //        return BadRequest(response.AsT1);
    //    }
    //}

    //[HttpPost, Route("AccountPhone_CreateToken")]
    //public async Task<ActionResult<string>> AccountPhone_CreateToken(string phoneNumber)
    //{
    //    var response = await _accountRepository.AccountPhone_CreateToken(phoneNumber);
    //    if (response.IsT0)
    //    {
    //        return Ok(response.AsT0);
    //    }
    //    else
    //    {
    //        return BadRequest(response.AsT1);
    //    }
    //}

    //[HttpPost, Route("AccountPhone_Change")]
    //public async Task<ActionResult<bool>> AccountPhone_Change(string token, string newPhoneNumber)
    //{
    //    var response = await _accountRepository.AccountPhone_Change(token, newPhoneNumber);
    //    if (response.IsT0)
    //    {
    //        return Ok(response.AsT0);
    //    }
    //    else
    //    {
    //        return BadRequest(response.AsT1);
    //    }
    //}

    //[HttpPost, Route("AccountPhone_Confirmed")]
    //public async Task<ActionResult<bool>> AccountPhone_Confirmed()
    //{
    //    var response = await _accountRepository.AccountPhone_Confirmed();
    //    if (response.IsT0)
    //    {
    //        return Ok(response.AsT0);
    //    }
    //    else
    //    {
    //        return BadRequest(response.AsT1);
    //    }
    //}

    //[HttpPost, Route("AccountPassword_Change")]
    //public async Task<ActionResult<bool>> AccountPassword_Change(string newPassword, string token)
    //{
    //    var response = await _accountRepository.AccountPassword_Change(newPassword, token);
    //    if (response.IsT0)
    //    {
    //        return Ok(response.AsT0);
    //    }
    //    else
    //    {
    //        return BadRequest(response.AsT1);
    //    }
    //}

    //[HttpPost, Route("AccountPassword_CreateToken")]
    //public async Task<ActionResult<string>> AccountPassword_CreateToken()
    //{
    //    var response = await _accountRepository.AccountPassword_CreateToken();
    //    if (response.IsT0)
    //    {
    //        return Ok(response.AsT0);
    //    }
    //    else
    //    {
    //        return BadRequest(response.AsT1);
    //    }
    //}

    //[HttpPost, Route("TwoFactorAuthSetup")]
    //public async Task<ActionResult<TfaSetupDto>> TwoFactorAuthSetup(string email)
    //{
    //    var response = await _accountRepository.TwoFactorAuthSetup(email);
    //    if (response.IsT0)
    //    {
    //        return Ok(response.AsT0);
    //    }
    //    else
    //    {
    //        return BadRequest(response.AsT1);
    //    }
    //}

    //[HttpPost, Route("PostTfaSetup")]
    //public async Task<ActionResult<TfaSetupDto>> PostTfaSetup(TfaSetupDto tfaModel)
    //{
    //    var response = await _accountRepository.PostTfaSetup(tfaModel);
    //    if (response.IsT0)
    //    {
    //        return Ok(response.AsT0);
    //    }
    //    else
    //    {
    //        return BadRequest(response.AsT1);
    //    }
    //}
}