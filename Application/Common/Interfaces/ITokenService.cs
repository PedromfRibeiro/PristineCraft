using PristineCraft.Domain.Entities.User;

namespace PristineCraft.Application.Common.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
}