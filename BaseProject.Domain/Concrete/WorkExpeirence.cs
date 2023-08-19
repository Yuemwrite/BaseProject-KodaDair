using Domain.Base;

namespace Domain.Concrete;

public class WorkExpeirence : FullAuditableEntity<long>
{
    public Guid UserId { get; set; }
    
    public string CompanyName { get; set; }
    
    public string? Remark { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }

    #region RelationEntities

    public virtual User User { get; set; }

    #endregion
    
}