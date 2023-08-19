using Domain.Base;
using Domain.Enum;
using Newtonsoft.Json;

namespace Domain.Concrete;

public class TwoFactorAuthenticationTransaction : FullAuditableEntity<long>
{
    public Guid UserId { get; set; }
    
    public OneTimePasswordType OneTimePasswordType { get; set; }
    
    public OneTimePasswordChannel OneTimePasswordChannel { get; set; }
    
    public string To { get; set; }
    
    public string OneTimePassword { get; set; } //Hash olacak şekilde tutuluyor.
    
    public bool IsUsed { get; set; } 
    
    public bool IsSend { get; set; }

    #region RelationEntities

    [JsonIgnore]
    public virtual TransactionUser TransactionUser { get; set; }

    #endregion
    
    
}