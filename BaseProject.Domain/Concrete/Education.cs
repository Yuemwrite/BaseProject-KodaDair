using Domain.Base;
using Domain.Enum;

namespace Domain.Concrete;

public class Education : FullAuditableEntity<long>
{
    public Guid UserId { get; set; }
    
    public EducationLevel EducationLevel { get; set; }
    
    public string SchoolName { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }

    #region RelationEntities

    public virtual User User { get; set; }

    #endregion
}