namespace PristineCraft.Application.DTO.User;

public class TfaSetupDto
{
    public string? Email { get; set; }
    public string? Code { get; set; }
    public bool IsTfaEnabled { get; set; }
    public string? AuthenticatorKey { get; set; }
    public string? FormattedKey { get; set; }

    private class Mapping : Profile
    {
        public Mapping() => CreateMap<AppUser, TfaSetupDto>().ReverseMap();
    }
}