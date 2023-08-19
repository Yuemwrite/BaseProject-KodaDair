using Domain.Base;
using Domain.Base.Auditing;
using Domain.Enum;

namespace Domain.Concrete;

public class Follower : FullAuditableEntity<Guid>, IPassivable
{
    public Guid FollowerUserId { get; set; }

    public Guid FollowedUserId { get; set; }
    public bool IsActive { get; set; }
    
    public ApprovalStatus ApprovalStatus { get; set; }

    #region RelationEntities

    public virtual User FollowerUser { get; set; }
    public virtual User FollowedUser { get; set; }

    #endregion
}