namespace BaseProject.Application.Identity.Tokens;

public class JwtSettings
{
    public string Key { get; set; }
    public int TokenExpirationInMinutes { get; set; }
    public int RefreshTokenExpirationInDays { get; set; }
    public bool AllowConcurrentSessions { get; set; }

    public string ValidAudience { get; set; }

    public string ValidIssuer { get; set; }
}