using Microsoft.AspNetCore.Identity;
using PristineCraft.Application.Common.Models;
using PristineCraft.Application.DTO.User;
using PristineCraft.Application.Services;
using PristineCraft.Domain.Entities.User;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Threading.Channels;

namespace PristineCraft.Application.Common.Interfaces;

public interface IAccountRepository
{
    Task<(SignInResult, LoginResponseDto?)> AccountLogin(LoginRequestDto userLogin);

    Task<bool> AccountPassword_Change(string newPassword, string token);

    Task<string> AccountPassword_CreateToken();

    Task<bool> AccountPhone_Change(string token, string newPhoneNumber);

    Task<bool> AccountPhone_Confirmed();

    Task<string> AccountPhone_CreateToken(string phoneNumber);

    Task<(IdentityResult, IdentityResult, RegisterResponseDTO)> AccountRegister(RegisterRequestDto request);

    Task<IdentityResult> AccountRole_Add(string email, string role);

    Task<IdentityResult> AccountRole_Remove(string email, string role);

    Task Delete(string email);

    Task<AppUser> Fetch(string email);

    Task<PaginatedList<AppUser>> Fetch(FilterParams filterParams);

    Task Logout();

    Task<TfaSetupDto> PostTfaSetup(TfaSetupDto tfaModel);

    //Task<(TfaSetupDto, Result)> TwoFactorAuthSetup(string email);
    Task Update(UpdateUserRequestDto request);
}