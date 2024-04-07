using Application.DTO.User;
using Application.Helper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using OneOf;
using OneOf.Types;

namespace Application.Interfaces;

public interface IAccountRepository
{
	Task<OneOf<(SignInResult, LoginResponseDto), Error<string>>> AccountLogin(LoginRequestDto userLogin);

	Task<OneOf<(IdentityResult, IdentityResult, RegisterResponseDTO), Error<string>>> AccountRegister(RegisterRequestDto request);

	Task Logout();

	Task<OneOf<User, NotFound>> Fetch(string email);

	Task<OneOf<PagedList<User>, NotFound>> Fetch(FilterParams filterParams);

	Task<OneOf<bool, Error<string>>> Update(UpdateUserRequestDto request);

	Task<OneOf<bool, Error<string>>> Delete(string email);

	Task<OneOf<IdentityResult, Error<string>>> AccountRole_Add(string email, string role);

	Task<OneOf<IdentityResult, Error<string>>> AccountRole_Remove(string email, string role);

	Task<OneOf<string, Error<string>>> AccountPhone_CreateToken(string phoneNumber);

	Task<OneOf<bool, Error<string>>> AccountPhone_Change(string token, string newPhoneNumber);

	Task<OneOf<bool, Error<string>>> AccountPhone_Confirmed();

	Task<OneOf<bool, Error<string>>> AccountPassword_Change(string newPassword, string token);

	Task<OneOf<string, Error<string>>> AccountPassword_CreateToken();

	Task<OneOf<TfaSetupDto, NotFound>> TwoFactorAuthSetup(string email);

	Task<OneOf<TfaSetupDto, NotFound, Error<string>>> PostTfaSetup(TfaSetupDto tfaModel);
}