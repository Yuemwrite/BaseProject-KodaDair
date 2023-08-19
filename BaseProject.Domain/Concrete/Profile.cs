using Domain.Base;

namespace Domain.Concrete;

public class Profile : FullAuditableEntity<Guid>
{
    public Guid UserId { get; set; }
    
    public string Title { get; set; }
    
    public string? Biography { get; set; }
    
    public string? SocialMediaAddress1 { get; set; }
    
    public string? SocialMediaAddress2 { get; set; }
    
    public string? WebSite { get; set; }

    #region RelationEntities

    public virtual User User { get; set; }

    #endregion
}