namespace Domain.Base.Auditing;

public interface ICreatable
{
    /// <summary>
    /// Creation time.
    /// </summary>
    DateTime CreationTime { get; set; }

    /// <summary>
    /// Id of the creator.
    /// </summary>
    Guid CreatorUserId { get; set; }
}