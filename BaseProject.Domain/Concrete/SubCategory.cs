using Domain.Base;

namespace Domain.Concrete;

public class SubCategory : FullAuditableEntity<long>
{
    public long CategoryId { get; set; }
    
    public string Name { get; set; }

    #region Relation

    public virtual Category Category { get; set; }

    public virtual ICollection<Sharing> Sharings { get; set; }

    #endregion
}