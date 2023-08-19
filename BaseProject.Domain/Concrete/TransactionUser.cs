using Domain.Base;

namespace Domain.Concrete;

public class TransactionUser : IEntity
{
    public Guid Id { get; set; }
    
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public string MobilePhoneNumber { get; set; }
    
    public string Password { get; set; }

    #region RelationEntites

    public virtual ICollection<TwoFactorAuthenticationTransaction> TwoFactorAuthenticationTransactions { get; set; }

    #endregion
}