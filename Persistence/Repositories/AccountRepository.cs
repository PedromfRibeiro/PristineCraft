using Application.DTO.User;
using Application.Exception;
using Application.Helper;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Persistence.Repositories;

public class AccountRepository(
							   UserManager<User> userManager,
							   SignInManager<User> signInManager,
							   IMapper mapper,
							   ITokenService tokenService,
							   UrlEncoder urlEncoder,
							   DataContext context) : IAccountRepository
{
	public async Task<OneOf<(SignInResult, LoginResponseDto), Error<string>>> AccountLogin(LoginRequestDto userLogin)
	{
		var userFound = await AccountFindByEmail(userLogin.Email);
		return userFound.Match<OneOf<(SignInResult, LoginResponseDto), Error<string>>>(
												user =>
												{
													var result = (signInManager.CheckPasswordSignInAsync(user, userLogin.Password, true).Result);
													if (result.Succeeded)
													{
														LoginResponseDto response = mapper.Map<LoginResponseDto>(user);
														response.Token = tokenService.CreateToken(user).Result;
														return (result, response);
													}
													else
													{
														return (result, null);
													}
												},
												notFound => new Error<string>(ExceptionStrings.NotFound_Email)
												);
	}

	public async Task<OneOf<(IdentityResult, IdentityResult, RegisterResponseDTO), Error<string>>> AccountRegister(RegisterRequestDto request)
	{
		if ((await AccountFindByEmail(request.Email)).IsT1)
			return new Error<string>(ExceptionStrings.Account_Register_EmailUsed);
		if ((await AccountFindByUserName(request.UserName)).IsT1)
			return new Error<string>(ExceptionStrings.Account_Register_UserNameUsed);
		User user = mapper.Map<User>(request);
		var userCreateResult = await userManager.CreateAsync(user, request.Password);
		if (userCreateResult.Succeeded)
		{
			var userRoleResult = await userManager.AddToRoleAsync(user, "Member");
			return (
				userRoleResult,
				userCreateResult,
				mapper.Map<RegisterResponseDTO>(AccountFindByEmail(request.Email))
				);
		}
		else
		{
			return new Error<string>(ExceptionStrings.Account_Register_Failed);
		}
	}

	public async Task Logout()
	{
		await signInManager.SignOutAsync();
	}

	public async Task<OneOf<User, NotFound>> Fetch(string email)
	{
		return await AccountFindByEmail(email);
	}

	public async Task<OneOf<PagedList<User>, NotFound>> Fetch(FilterParams filterParams)
	{
		var query = context.DbUser.AsNoTracking();
		return await PagedList<User>.CreateAsync(query, filterParams._pageNumber, filterParams.PageSize, filterParams.FilterOptions2);
	}

	public async Task<OneOf<bool, Error<string>>> Update(UpdateUserRequestDto request)
	{
		var email = System.Security.Claims.ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
		OneOf<User, NotFound> accountFindEmail = await AccountFindByEmail(email);
		if (accountFindEmail.IsT1)
			return new Error<string>(ExceptionStrings.Server_Failed);

		mapper.Map(request, accountFindEmail.AsT0);
		context.Entry(accountFindEmail.AsT0).State = EntityState.Modified;
		return await context.SaveChangesAsync() > 0 ? true : new Error<string>(ExceptionStrings.Server_Update_Failed);
	}

	public async Task<OneOf<bool, Error<string>>> Delete(string email)
	{
		OneOf<User, NotFound> accountFindEmail = await AccountFindByEmail(email);
		if (accountFindEmail.IsT1)
			return new Error<string>(ExceptionStrings.Server_Failed);
		context.Entry(accountFindEmail.AsT0).State = EntityState.Deleted;
		return await context.SaveChangesAsync() > 0 ? true : new Error<string>(ExceptionStrings.Server_Delete_Failed);
	}

	public async Task<OneOf<IdentityResult, Error<string>>> AccountRole_Add(string email, string role)
	{
		var userFound = await AccountFindByEmail(email);
		return userFound.Match<OneOf<IdentityResult, Error<string>>>(
												user => userManager.AddToRoleAsync(user, role).Result,
												notFound => new Error<string>(ExceptionStrings.NotFound_Email)
												);
	}

	public async Task<OneOf<IdentityResult, Error<string>>> AccountRole_Remove(string email, string role)
	{
		var userFound = await AccountFindByEmail(email);
		return userFound.Match<OneOf<IdentityResult, Error<string>>>(
												user => userManager.RemoveFromRoleAsync(user, role).Result,
												notFound => new Error<string>(ExceptionStrings.NotFound_Email)
												);
	}

	public async Task<OneOf<string, Error<string>>> AccountPhone_CreateToken(string phoneNumber)
	{
		var email = System.Security.Claims.ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
		User? user = await userManager.FindByEmailAsync(email);
		if (user == null)
			return new Error<string>(ExceptionStrings.NotFound_Email);
		string tokenResult = await userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
		if (String.IsNullOrEmpty(tokenResult))
			return new Error<string>(ExceptionStrings.Account_TokenFailed);
		return tokenResult;
	}

	public async Task<OneOf<bool, Error<string>>> AccountPhone_Change(string token, string newPhoneNumber)
	{
		var email = System.Security.Claims.ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
		User? user = await userManager.FindByEmailAsync(email);
		if (user == null)
			return new Error<string>(ExceptionStrings.NotFound_Email);
		IdentityResult result = await userManager.ChangePhoneNumberAsync(user, newPhoneNumber, token);
		if (result.Succeeded)
			return true;
		return new Error<string>(ExceptionStrings.Account_Phone_Failed);
	}

	public async Task<OneOf<bool, Error<string>>> AccountPhone_Confirmed()
	{
		var email = System.Security.Claims.ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
		User? user = await userManager.FindByEmailAsync(email);
		if (user == null)
			return new Error<string>(ExceptionStrings.NotFound_Email);

		return await userManager.IsPhoneNumberConfirmedAsync(user);
	}

	public async Task<OneOf<bool, Error<string>>> AccountPassword_Change(string newPassword, string token)
	{
		var email = System.Security.Claims.ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
		User? user = await userManager.FindByEmailAsync(email);
		if (user == null)
			return new Error<string>(ExceptionStrings.NotFound_Email);
		IdentityResult identityResult = await userManager.ResetPasswordAsync(user, token, newPassword);
		if (identityResult.Succeeded)
			return true;
		else
			return new Error<string>(ExceptionStrings.Account_Password_Failed);
	}

	public async Task<OneOf<string, Error<string>>> AccountPassword_CreateToken()
	{
		var email = System.Security.Claims.ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
		User? user = await userManager.FindByEmailAsync(email);
		if (user == null)
			return new Error<string>(ExceptionStrings.NotFound_Email);

		string resultToken = await userManager.GeneratePasswordResetTokenAsync(user);
		if (String.IsNullOrEmpty(resultToken))
			return new Error<string>(ExceptionStrings.Account_TokenFailed);

		return resultToken;
	}

	public async Task<OneOf<TfaSetupDto, NotFound>> TwoFactorAuthSetup(string email)
	{
		var userFound = await AccountFindByEmail(email);

		return userFound.Match<OneOf<TfaSetupDto, NotFound>>(
												user =>
												{
													var isTfaEnabled = userManager.GetTwoFactorEnabledAsync(user);
													var authenticatorKey = userManager.GetAuthenticatorKeyAsync(user);
													if (authenticatorKey == null)
													{
														userManager.ResetAuthenticatorKeyAsync(user);
														authenticatorKey = userManager.GetAuthenticatorKeyAsync(user);
													}
													var formattedKey = GenerateQRCode(email, authenticatorKey.Result);
													return new TfaSetupDto
													{
														IsTfaEnabled = isTfaEnabled.Result,
														AuthenticatorKey = authenticatorKey.Result,
														FormattedKey = formattedKey
													};
												},
												notFound => new NotFound()
												);
	}

	public async Task<OneOf<TfaSetupDto, NotFound, Error<string>>> PostTfaSetup(TfaSetupDto tfaModel)
	{
		var userFound = await AccountFindByEmail(tfaModel.Email);

		return userFound.Match<OneOf<TfaSetupDto, NotFound, Error<string>>>(
												user =>
												{
													var isValidCode = userManager.VerifyTwoFactorTokenAsync(user,
																  userManager.Options.Tokens.AuthenticatorTokenProvider,
																  tfaModel.Code);
													if (isValidCode.Result)
													{
														userManager.SetTwoFactorEnabledAsync(user, true);
														return new TfaSetupDto { IsTfaEnabled = true };
													}
													else
													{
														return new Error<string>("Invalid code");
													}
												},
												notFound => new NotFound()
												);
	}

	private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

	internal string GenerateQRCode(string email, string unformattedKey)
	{
		return string.Format(
		AuthenticatorUriFormat,
			urlEncoder.Encode(" Two-Factor Auth"),
			urlEncoder.Encode(email),
			unformattedKey);
	}

	internal async Task<OneOf<User, NotFound>> AccountFindByEmail(string email, bool throwException = false)
	{
		User? user = await userManager.FindByEmailAsync(email);
		if (user == null)
			return new NotFound();
		return user;
	}

	internal async Task<OneOf<User, NotFound>> AccountFindByUserName(string username, bool throwException = false)
	{
		User? user = await userManager.FindByNameAsync(username);
		if (user == null)
			return new NotFound();
		return user;
	}

	internal async Task<OneOf<User, NotFound>> AccountFindByProvider(string loginProvider, string providerKey, string loginProviderName, bool throwException = false)
	{
		User? user = await userManager.FindByLoginAsync(loginProvider, providerKey);
		if (user == null)
			return new NotFound();
		return user;
	}
}