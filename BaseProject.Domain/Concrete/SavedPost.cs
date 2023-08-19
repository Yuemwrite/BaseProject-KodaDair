using Domain.Base;
using Domain.Base.Auditing;

namespace Domain.Concrete;

public class SavedPost : FullAuditableEntity<Guid>, IPassivable
{
    public Guid UserId { get; set; }
    
    public Guid SharingId { get; set; }
    public bool IsActive { get; set; }

    #region RelationEntities

    public virtual User User { get; set; }

    public virtual Sharing Sharing { get; set; }
    
    #endregion
}