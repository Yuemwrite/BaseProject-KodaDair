using System.Collections;
using Domain.Base;
using Domain.Enum;

namespace Domain.Concrete;

public class Sharing : FullAuditableEntity<Guid>
{
    public Guid UserId { get; set; }

    public SharingType SharingType { get; set; }
    
    public string Title { get; set; }
    
    public string Content { get; set; }
    
    public long CategoryId { get; set; }
    
    public long? SubCategoryId { get; set; }
    
    public bool IsFixed { get; set; }
    

    #region RelationEntities

    public virtual User User { get; set; }
    public virtual Category Category { get; set; }
    public virtual SubCategory SubCategory { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Like> Likes { get; set; }
    public virtual ICollection<SavedPost> SavedPosts { get; set; }


    #endregion
}