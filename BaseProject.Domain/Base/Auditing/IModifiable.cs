namespace Domain.Base.Auditing;

public interface IModifiable : IRowVersion
{
    /// <summary>
    /// The last modified time for this entity.
    /// </summary>
    DateTime? LastModificationTime { get; set; }

    /// <summary>
    /// Last modifier user for this entity.
    /// </summary>
    Guid? LastModifierUserId { get; set; }
}