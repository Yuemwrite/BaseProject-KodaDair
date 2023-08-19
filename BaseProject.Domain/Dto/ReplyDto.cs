namespace Domain.Dto;

public class ReplyDto : BaseDto.BaseDto
{
    public Guid Id { get; set; }
    
    public Guid CommentId { get; set; }
    
    public string Content { get; set; }
    
    public int LikeCount { get; set; }
    
}