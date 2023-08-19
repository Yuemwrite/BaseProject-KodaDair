using Domain.Concrete;
using Newtonsoft.Json;

namespace Domain.Dto;

public class CommentDto : BaseDto.BaseDto
{
    public Guid Id { get; set; }
    public Guid SharingId { get; set; }
    public string Content { get; set; }
    public int LikeCount { get; set; }
    public int ReplyCount { get; set; }
    
    
}