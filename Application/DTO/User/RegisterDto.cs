using System.ComponentModel.DataAnnotations;

namespace PristineCraft.Application.DTO.User;

public class RegisterRequestDto
{
    [Required]
    public required string Name { get; set; }
    public required string Contact { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Observations { get; set; }

    //IdentityUser Fields
    [Required]
    public required string UserName { get; set; }

    [Required]
    public required string Email { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 4)]
    public required string Password { get; set; }

    private class Mapping : Profile
    {
        public Mapping() => CreateMap<Domain.Entities.User.AppUser, RegisterRequestDto>().ReverseMap();
    }
}

public class RegisterResponseDTO
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public bool RoleCreationSuccess { get; set; }
    public string? RoleCreationError { get; set; }

    private class Mapping : Profile
    {
        public Mapping() => CreateMap<Domain.Entities.User.AppUser, RegisterResponseDTO>().ReverseMap();
    }
}