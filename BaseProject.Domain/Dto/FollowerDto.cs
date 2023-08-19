using Domain.Enum;

namespace Domain.Dto;

public class FollowerDto : BaseDto.BaseDto
{
    public Guid Id { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; }
    
    public Guid FollowerUserId { get; set; }
    
    public string FollowerUserName { get; set; }
}