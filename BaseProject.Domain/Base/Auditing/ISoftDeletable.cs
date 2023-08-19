namespace Domain.Base.Auditing;

public interface ISoftDeletable
{
    /// <summary>
    /// Deleter User Id.
    /// </summary>
    Guid? DeleterUserId { get; set; }
    /// <summary>
    /// Deletion time.
    /// </summary>
    DateTime? DeletionTime { get; set; }
    
    /// <summary>
    /// Determines if the entity is deleted or not
    /// </summary>
    bool IsDeleted { get; set; }
    /// <summary>
    /// The reason description for deletion.
    /// </summary>
    string DeletionReason { get; set; }
}