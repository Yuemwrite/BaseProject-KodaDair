namespace Domain.Dto;

public class UserProfileDto : BaseDto.BaseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Biography { get; set; }

    public string? SocialMediaAddress1 { get; set; }
    public string? SocialMediaAddress2 { get; set; }
    public string? WebSite { get; set; }
    public int FollowerCount { get; set; }
    public int FollowedCount { get; set; }
    
}