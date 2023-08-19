using Domain.Base;

namespace Domain.Concrete;

public class Product : FullAuditableEntity<long>
{
    public string Name { get; set; }
    
    public string Code { get; set; }

    public string Description { get; set; }
    
    public int Stock { get; set; }
    
    public decimal Amount { get; set; }
    
    public Guid UserId { get; set; }
    
    public virtual User User { get; set; }
}