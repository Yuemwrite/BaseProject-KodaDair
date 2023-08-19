namespace Domain.Dto;

public class LikeDto : BaseDto.BaseDto
{
    public long Id { get; set; }
    
    
    public Guid? SharingId { get; set; }
    
    public Guid? CommentId { get; set; }
    
    public Guid? ReplyId { get; set; }
    
    public bool IsActive { get; set; }
    

}