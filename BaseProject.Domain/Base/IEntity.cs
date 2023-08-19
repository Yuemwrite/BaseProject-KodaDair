using Domain.Base.Auditing;

namespace Domain.Base;

public interface IEntity<TPrimaryKey> : IEntity, IRowVersion
{
    TPrimaryKey Id { get; set; }
    Guid UID { get; set; }
    byte[] RowVersion { get; set; }
}
public interface IEntity
{ }