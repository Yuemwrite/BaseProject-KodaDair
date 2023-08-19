namespace Domain.Base.Auditing;

public interface IApproval
{
    Guid? UserId { get; set; }
    
    bool IsApproved { get; set; }
    
    string? Remark { get; set; }
    
    DateTime? ApprovedTime { get; set; }
}