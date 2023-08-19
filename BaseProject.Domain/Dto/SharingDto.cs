using Domain.Concrete;
using Domain.Enum;

namespace Domain.Dto;

public class SharingDto : BaseDto.BaseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsFixed { get; set; }
    public int CommentCount { get; set; }
    public int LikeCount { get; set; }
    
}