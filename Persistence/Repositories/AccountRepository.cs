using Application.DTO.User;
using Application.Exception;
using Application.Helper;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using System.Text.Encodings.Web;

namespace Persistence.Repositories;
public class AccountRepository : IAccountRepository
{
	private readonly IMapper _mapper;
	private readonly UserManager<User> _userManager;
	private readonly SignInManager<User> _signInManager;
	private readonly UrlEncoder _urlEncoder;
	private readonly DataContext _context;

	public AccountRepository(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, UrlEncoder urlEncoder, DataContext context)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_mapper = mapper;
		_urlEncoder = urlEncoder;
		_context = context;
	}


	public async Task<OneOf<User, NotFound>> Fetch(string email)
	{
		return await AccountFindByEmail(email);
	}
	public async Task<OneOf<PagedList<User>, NotFound>> Fetch(FilterParams filterParams)
	{
		var query = _context.db_User.AsNoTracking();

		return await PagedList<User>.CreateAsync(query, filterParams.PageNumber, filterParams.PageSize, filterParams.FilterOptions2);

	}
	public async Task<bool> Update(User request)
	{
		_context.Entry(request).State = EntityState.Modified;
		return await _context.SaveChangesAsync() > 0;
	}
	public async Task<bool> Delete(User request)
	{
		_context.Entry(request).State = EntityState.Deleted;
		return await _context.SaveChangesAsync() > 0;
	}

	public async Task<OneOf<(SignInResult, User), Error<string>>> AccountLogin(LoginRequestDto userLogin)
	{
		var userFound = await AccountFindByEmail(userLogin.Email);
		return userFound.Match<OneOf<(SignInResult, User), Error<string>>>(
												user => (_signInManager.CheckPasswordSignInAsync(user, userLogin.Password, true).Result, user),
												notFound => new Error<string>(ExceptionStrings.NotFound_Email)
												);
	}

	public async Task AccountLogOut()
	{
		await _signInManager.SignOutAsync();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="request"></param>
	/// <returns>
	/// OneOf:	(IdentityResult of Roles, IdentityResult of User, number of items saved) Or Error string
	/// </returns>
	public async Task<OneOf<(IdentityResult, IdentityResult, int), Error<string>>> AccountRegister(RegisterRequestDto request)
	{
		if ((await AccountFindByEmail(request.Email)).IsT1)
			return new Error<string>(ExceptionStrings.Account_Register_EmailUsed);
		if ((await AccountFindByUserName(request.UserName)).IsT1)
			return new Error<string>(ExceptionStrings.Account_Register_UserNameUsed);


		User user = new User();
		_mapper.Map(request, user);

		var roleResult = await _userManager.AddToRoleAsync(user, "Member");
		var result = await _userManager.CreateAsync(user, request.Password);
		var saveResult = await _context.SaveChangesAsync();
		return (roleResult, result, saveResult);

	}



	public async Task<OneOf<IdentityResult, Error<string>>> AccountRole_Add(string email, string role)
	{
		var userFound = await AccountFindByEmail(email);
		return userFound.Match<OneOf<IdentityResult, Error<string>>>(
												user => _userManager.AddToRoleAsync(user, role).Result,
												notFound => new Error<string>(ExceptionStrings.NotFound_Email)
												);
	}
	public async Task<OneOf<IdentityResult, Error<string>>> AccountRole_Remove(string email, string role)
	{
		var userFound = await AccountFindByEmail(email);
		return userFound.Match<OneOf<IdentityResult, Error<string>>>(
												user => _userManager.RemoveFromRoleAsync(user, role).Result,
												notFound => new Error<string>(ExceptionStrings.NotFound_Email)
												);
	}


	public async Task<OneOf<string, Error<string>>> AccountPhone_CreateToken(string Email, string phoneNumber)
	{
		User? user = await _userManager.FindByEmailAsync(Email);
		if (user == null)
			return new Error<string>(ExceptionStrings.NotFound_Email);
		string tokenResult = await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
		if (String.IsNullOrEmpty(tokenResult))
			return new Error<string>(ExceptionStrings.Account_TokenFailed);
		return tokenResult;
	}
	public async Task<OneOf<bool, Error<string>>> AccountPhone_Change(string Email, string token, string newPhoneNumber)
	{
		User? user = await _userManager.FindByEmailAsync(Email);
		if (user == null)
			return new Error<string>(ExceptionStrings.NotFound_Email);
		IdentityResult result = await _userManager.ChangePhoneNumberAsync(user, newPhoneNumber, token);
		if (result.Succeeded)
			return true;
		return new Error<string>(ExceptionStrings.Account_Phone_Failed);
	}
	public async Task<OneOf<bool, Error<string>>> AccountPhone_Confirmed(string Email)
	{
		User? user = await _userManager.FindByEmailAsync(Email);
		if (user == null)
			return new Error<string>(ExceptionStrings.NotFound_Email);

		return await _userManager.IsPhoneNumberConfirmedAsync(user);
	}


	public async Task<OneOf<bool, Error<string>>> AccountPassword_Change(string Email, string newPassword, string token)
	{
		User? user = await _userManager.FindByEmailAsync(Email);
		if (user == null)
			return new Error<string>(ExceptionStrings.NotFound_Email);
		IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, token, newPassword);
		if (identityResult.Succeeded)
			return true;
		else
			return new Error<string>(ExceptionStrings.Account_Password_Failed);
	}
	public async Task<OneOf<string, Error<string>>> AccountPassword_CreateToken(string Email)
	{
		User? user = await _userManager.FindByEmailAsync(Email);
		if (user == null)
			return new Error<string>(ExceptionStrings.NotFound_Email);

		string resultToken = await _userManager.GeneratePasswordResetTokenAsync(user);
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
													var isTfaEnabled = _userManager.GetTwoFactorEnabledAsync(user);
													var authenticatorKey = _userManager.GetAuthenticatorKeyAsync(user);
													if (authenticatorKey == null)
													{
														_userManager.ResetAuthenticatorKeyAsync(user);
														authenticatorKey = _userManager.GetAuthenticatorKeyAsync(user);
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
													var isValidCode = _userManager.VerifyTwoFactorTokenAsync(user,
																  _userManager.Options.Tokens.AuthenticatorTokenProvider,
																  tfaModel.Code);
													if (isValidCode.Result)
													{
														_userManager.SetTwoFactorEnabledAsync(user, true);
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
			_urlEncoder.Encode(" Two-Factor Auth"),
			_urlEncoder.Encode(email),
			unformattedKey);
	}


	internal async Task<OneOf<User, NotFound>> AccountFindByEmail(string email, bool throwException = false)
	{
		User? user = await _userManager.FindByEmailAsync(email);
		if (user == null)
			return new NotFound();
		return user;
	}
	internal async Task<OneOf<User, NotFound>> AccountFindByUserName(string username, bool throwException = false)
	{
		User? user = await _userManager.FindByNameAsync(username);
		if (user == null)
			return new NotFound();
		return user;
	}
	internal async Task<OneOf<User, NotFound>> AccountFindByProvider(string loginProvider, string providerKey, string loginProviderName, bool throwException = false)
	{
		User? user = await _userManager.FindByLoginAsync(loginProvider, providerKey);
		if (user == null)
			return new NotFound();
		return user;
	}
}

