namespace Domain.Base.Auditing;

public interface IPassivable
{
    /// <summary>
    /// True: This entity is active.
    /// False: This entity is not active.
    /// </summary>
    bool IsActive { get; set; }
}