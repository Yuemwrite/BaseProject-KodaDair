using Domain.Base;

namespace Domain.Concrete;

public class Reply : FullAuditableEntity<Guid>
{
    public Guid UserId { get; set; }
    
    public Guid CommentId { get; set; }
    
    public string Content { get; set; }

    #region RelationEntities

    public virtual User User { get; set; }
    
    public virtual Comment Comment { get; set; }
    
    public virtual ICollection<Like> Likes { get; set; }

    #endregion
}