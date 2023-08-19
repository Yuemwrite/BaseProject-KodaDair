using System.Collections;
using Domain.Base;

namespace Domain.Concrete;

public class Comment : FullAuditableEntity<Guid>
{
    public Guid UserId { get; set; }
    
    public Guid SharingId { get; set; }
    
    public string Content { get; set; }

    #region RelationEntities

    public virtual User User { get; set; }
    public virtual Sharing Sharing { get; set; }
    public virtual ICollection<Reply> Replies { get; set; }
    
    public virtual ICollection<Like> Likes { get; set; }

    #endregion
    
}