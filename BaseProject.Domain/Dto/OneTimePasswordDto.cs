namespace Domain.Dto;

public class OneTimePasswordDto
{
    public bool Success { get; set; }
    public string OneTimePassword { get; set; }
    public long OneTimePasswordId { get; set; }
    public Guid UserId { get; set; }
}