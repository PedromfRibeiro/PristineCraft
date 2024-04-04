using AutoMapper;
using Domain.Entities;
using Application.DTO.User;

namespace Shared.Helper;
public class AutoMapperProfiles : Profile
{
	public AutoMapperProfiles()
	{
		CreateMap<User, LoginRequestDto>().ReverseMap();
		CreateMap<User, LoginResponseDto>().ReverseMap();
		CreateMap<User, RegisterRequestDto>().ReverseMap();
		CreateMap<User, RegisterResponseDTO>().ReverseMap();
		CreateMap<User, TfaSetupDto>().ReverseMap();
	}
}
