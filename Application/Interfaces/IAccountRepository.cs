using Application.DTO.User;
using Application.Helper;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using OneOf;
using OneOf.Types;

namespace Application.Interfaces;

public interface IAccountRepository
{
	Task<OneOf<User, NotFound>> Fetch(string email);
	Task<OneOf<PagedList<User>, NotFound>> Fetch(FilterParams filterParams);
	Task<bool> Update(User request);
	Task<bool> Delete(User request);
	Task<OneOf<(SignInResult, User), Error<string>>> AccountLogin(LoginRequestDto userLogin);
	Task AccountLogOut();
	Task<OneOf<IdentityResult, Error<string>>> AccountRole_Add(string email, string role);
	Task<OneOf<IdentityResult, Error<string>>> AccountRole_Remove(string email, string role);
	Task<OneOf<string, Error<string>>> AccountPhone_CreateToken(string Email, string phoneNumber);
	Task<OneOf<bool, Error<string>>> AccountPhone_Change(string Email, string token, string newPhoneNumber);
	Task<OneOf<bool, Error<string>>> AccountPhone_Confirmed(string Email);
	Task<OneOf<bool, Error<string>>> AccountPassword_Change(string Email, string newPassword, string token);
	Task<OneOf<string, Error<string>>> AccountPassword_CreateToken(string Email);
	Task<OneOf<TfaSetupDto, NotFound>> TwoFactorAuthSetup(string email);
	Task<OneOf<TfaSetupDto, NotFound, Error<string>>> PostTfaSetup(TfaSetupDto tfaModel);

}
