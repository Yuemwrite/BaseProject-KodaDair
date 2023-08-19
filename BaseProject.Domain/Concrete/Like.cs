using Domain.Base;
using Domain.Base.Auditing;

namespace Domain.Concrete;

public class Like : FullAuditableEntity<long>, IPassivable
{
    public Guid UserId { get; set; }
    
    public Guid? SharingId { get; set; }
    public Guid? CommentId { get; set; }
    public Guid? ReplyId { get; set; }
    public bool IsActive { get; set; }

    #region RelationEntities

    public virtual User User { get; set; }
    public virtual Sharing Sharing { get; set; }
    public virtual Comment Comment { get; set; }
    public virtual Reply Reply { get; set; }

    #endregion

    
}