namespace Domain.Base.Auditing;

public interface IRowVersion
{
    /// <summary>
    /// The last modification timestamp for this entity
    /// </summary>
    byte[] RowVersion { get; set; }
}