using PristineCraft.Domain.Entities;

namespace PristineCraft.Application.DTO.User;

public class UpdateUserRequestDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public bool? EmailConfirmed { get; set; }
    public string? NormalizedEmail { get; set; }
    public string? UserName { get; set; }
    public string? NormalizedUserName { get; set; }
    public string? Contact { get; set; }
    public byte[]? ImageSmall { get; set; }
    public string? Observations { get; set; }
    public string? Token { get; set; }
    public int? CompanyId { get; set; }
    public bool? TwoFactorEnabled { get; set; }
    public EnumGender? Gender { get; set; }

    private class Mapping : Profile
    {
        public Mapping() => CreateMap<Domain.Entities.User.AppUser, UpdateUserRequestDto>().ReverseMap();
    }
}