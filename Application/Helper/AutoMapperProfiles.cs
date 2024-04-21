using AutoMapper;
using PristineCraft.Application.DTO.User;
using PristineCraft.Domain.Entities.User;

namespace PristineCraft.Application.Helper;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, LoginRequestDto>().ReverseMap();
        CreateMap<AppUser, LoginResponseDto>().ReverseMap();
        CreateMap<AppUser, RegisterRequestDto>().ReverseMap();
        CreateMap<AppUser, RegisterResponseDTO>().ReverseMap();
        CreateMap<AppUser, TfaSetupDto>().ReverseMap();
    }
}