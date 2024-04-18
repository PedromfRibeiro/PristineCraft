using Application.DTO.Event;
using Application.DTO.User;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Event;

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

		//Event
		CreateMap<Event, EventDto>().ReverseMap();
		CreateMap<Event, MinimalEventDto>().ReverseMap();
		CreateMap<Event, CreateEventDto>().ReverseMap();
		CreateMap<Event, UpdateEvent>().ReverseMap();
	}
}