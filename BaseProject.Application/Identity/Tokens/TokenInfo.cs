namespace BaseProject.Application.Identity.Tokens;

public class TokenInfo
{
    public string AccessToken { get; set; }
    
    public DateTime ExpirationTime { get; set; }
    
    public string RefreshToken { get; set; }
}