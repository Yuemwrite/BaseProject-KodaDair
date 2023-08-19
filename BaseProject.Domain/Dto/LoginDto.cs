namespace Domain.Dto;

public class LoginDto : BaseDto.BaseDto
{
    public string Token { get; set; }
    
    public string RefreshToken { get; set; }
    
    public DateTime TokenExpirationTime { get; set; }
}