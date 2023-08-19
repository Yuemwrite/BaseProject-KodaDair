namespace BaseProject.Application.Identity.Tokens;

public record TokenResponse(string Token, DateTime TokenExpiryTime, string RefreshToken, DateTime RefreshTokenExpiryTime);