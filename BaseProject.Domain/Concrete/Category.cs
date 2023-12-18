using Domain.Base;
using Domain.Base.Auditing;

namespace Domain.Concrete;

public class Category : FullAuditableEntity<long>
{
    //Test branch
    public string Name { get; set; }

    #region RelationEntities

    public virtual ICollection<SubCategory> SubCategories { get; set; }
    public virtual ICollection<Sharing> Sharings { get; set; }

    #endregion
}