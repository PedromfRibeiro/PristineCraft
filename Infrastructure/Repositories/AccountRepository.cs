using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PristineCraft.Application.Common.Interfaces;
using PristineCraft.Application.Common.Models;
using PristineCraft.Application.Common.Resources;
using PristineCraft.Application.DTO.User;
using PristineCraft.Domain.Entities.User;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Infrastructure.Repositories;

public class AccountRepository(
                               UserManager<AppUser> userManager,
                               SignInManager<AppUser> signInManager,
                               IMapper mapper,
                               ITokenService tokenService,
                               UrlEncoder urlEncoder,
                               DataContext context,
                               ResourceManager resourceManager) : IAccountRepository
{
    public ResourceManager ResourceManager { get; } = resourceManager;

    private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

    internal string GenerateQRCode(string email, string unformattedKey)
    {
        return string.Format(AuthenticatorUriFormat, urlEncoder.Encode(" Two-Factor Auth"), urlEncoder.Encode(email), unformattedKey);
    }

    private async Task<AppUser> AccountFindByClaims()
    {
        //TODO: NULL CHECK
        string? email = (ClaimsPrincipal.Current?.FindFirst(type: ClaimTypes.Email ?? "")?.Value) ?? throw new Exception("NotFound_Email");
        AppUser? user = await userManager.FindByEmailAsync(email);
        return user == null ? throw new Exception("NotFound_Email") : user;
    }

    private async Task<AppUser> AccountFindByEmail(string email, bool throwException = false)
    {
        AppUser? user = await userManager.FindByEmailAsync(email);
        return user == null ? throw new Exception("NotFound_Email") : user;
    }

    private async Task<AppUser?> AccountFindByUserName(string username, bool throwException = false)
    {
        AppUser? user = await userManager.FindByNameAsync(username);
        return user == null ? throw new Exception("NotFound_Username") : user;
    }

    private async Task<AppUser?> AccountFindByProvider(string loginProvider, string providerKey, string loginProviderName, bool throwException = false)
    {
        AppUser? user = await userManager.FindByLoginAsync(loginProvider, providerKey);
        return user == null ? throw new Exception("NotFound_LoginProvider") : user;
    }

    public async Task<(SignInResult, LoginResponseDto?)> AccountLogin(LoginRequestDto userLogin)
    {
        AppUser? userFound = await AccountFindByEmail(userLogin.Email);
        if (userFound == null)
            throw new Exception("NotFound_Email");
        else
        {
            var result = signInManager.CheckPasswordSignInAsync(userFound, userLogin.Password, true).Result;
            if (result.Succeeded)
            {
                LoginResponseDto response = mapper.Map<LoginResponseDto>(userFound);
                response.Token = tokenService.CreateToken(userFound).Result;
                return (result, response);
            }
            else
            {
                return (result, null);
            }
        }
    }

    public async Task<bool> AccountPassword_Change(string newPassword, string token)
    {
        AppUser user = await AccountFindByClaims();
        IdentityResult identityResult = await userManager.ResetPasswordAsync(user, token, newPassword);
        if (identityResult.Succeeded)
            return true;
        else
            throw new Exception("Account_Password_Failed");
    }

    public async Task<string> AccountPassword_CreateToken()
    {
        AppUser user = await AccountFindByClaims();
        string resultToken = await userManager.GeneratePasswordResetTokenAsync(user);
        if (string.IsNullOrEmpty(resultToken))
            throw new Exception("Account_TokenFailed");
        return resultToken;
    }

    public async Task<bool> AccountPhone_Change(string token, string newPhoneNumber)
    {
        AppUser user = await AccountFindByClaims();
        IdentityResult result = await userManager.ChangePhoneNumberAsync(user, newPhoneNumber, token);
        if (result.Succeeded)
            return true;
        throw new Exception("Account_Phone_Failed");
    }

    public async Task<bool> AccountPhone_Confirmed()
    {
        AppUser user = await AccountFindByClaims();
        return await userManager.IsPhoneNumberConfirmedAsync(user);
    }

    public async Task<string> AccountPhone_CreateToken(string phoneNumber)
    {
        AppUser user = await AccountFindByClaims();
        string tokenResult = await userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
        if (string.IsNullOrEmpty(tokenResult))
            throw new Exception("Account_TokenFailed");
        return tokenResult;
    }

    public async Task<(IdentityResult, IdentityResult, RegisterResponseDTO)> AccountRegister(RegisterRequestDto request)
    {
        if ((await AccountFindByEmail(request.Email)) != null)
            throw new Exception("Account_Register_EmailUsed");
        if ((await AccountFindByUserName(request.UserName)) != null)
            throw new Exception("Account_Register_UserNameUsed");

        AppUser user = mapper.Map<AppUser>(request);
        var userCreateResult = await userManager.CreateAsync(user, request.Password);
        if (userCreateResult.Succeeded)
        {
            var userRoleResult = await userManager.AddToRoleAsync(user, "Member");
            return (userRoleResult,
                       userCreateResult,
                       mapper.Map<RegisterResponseDTO>(AccountFindByEmail(request.Email)));
        }
        else
        {
            throw new Exception("Account_Register_Failed");
        }
    }

    public async Task<IdentityResult> AccountRole_Add(string email, string role)
    {
        AppUser? userFound = await AccountFindByEmail(email);
        return userManager.AddToRoleAsync(userFound, role).Result;
    }

    public async Task<IdentityResult> AccountRole_Remove(string email, string role)
    {
        AppUser? userFound = await AccountFindByEmail(email);
        return userManager.RemoveFromRoleAsync(userFound, role).Result;
    }

    public async Task Delete(string email)
    {
        AppUser? userFound = await AccountFindByEmail(email);
        context.Entry(userFound).State = EntityState.Deleted;
    }

    public async Task<AppUser> Fetch(string email)
    {
        return await AccountFindByEmail(email);
    }

    public async Task<PaginatedList<AppUser>> Fetch(FilterParams filterParams)
    {
        return await PaginatedList<AppUser>.CreateAsync(context.DbUser.AsNoTracking(), filterParams.PageNumber, filterParams.PageSize, filterParams.FilterOptions2);
    }

    public async Task Logout()
    {
        await signInManager.SignOutAsync();
    }

    public async Task<TfaSetupDto> PostTfaSetup(TfaSetupDto tfaModel)
    {
        //TODO: NULL CHECK
        var userFound = await AccountFindByEmail(tfaModel.Email ?? "");
        var isValidCode = userManager.VerifyTwoFactorTokenAsync(userFound, userManager.Options.Tokens.AuthenticatorTokenProvider, tfaModel.Code ?? "");
        if (isValidCode.Result)
        {
            await userManager.SetTwoFactorEnabledAsync(userFound, true);
            return new TfaSetupDto { IsTfaEnabled = true };
        }
        else
        {
            throw new Exception("Invalid code");
        }
    }

    //public async Task<(TfaSetupDto, Result)> TwoFactorAuthSetup(string email)
    //{
    //    var userFound = await AccountFindByEmail(email);

    //    var isTfaEnabled = userManager.GetTwoFactorEnabledAsync(userFound);
    //    string authenticatorKey = userManager.GetAuthenticatorKeyAsync(userFound).Result;
    //    if (authenticatorKey == null)
    //    {
    //        await userManager.ResetAuthenticatorKeyAsync(userFound);
    //        authenticatorKey = userManager.GetAuthenticatorKeyAsync(userFound);
    //    }
    //    var formattedKey =  GenerateQRCode(email, authenticatorKey);
    //    return new TfaSetupDto{IsTfaEnabled = isTfaEnabled.Result,AuthenticatorKey = authenticatorKey.Result,FormattedKey = formattedKey};

    //}

    public async Task Update(UpdateUserRequestDto request)
    {
        AppUser user = await AccountFindByClaims();
        mapper.Map(request, user);
        context.Entry(user).State = EntityState.Modified;
    }
}