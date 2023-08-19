using Domain.Base.Auditing;

namespace Domain.Base;

public abstract class FullAuditableEntity : FullAuditableEntity<int>
{ }
public abstract class SoftDeletableEntity : SoftDeletableEntity<int>
{ }
public abstract class ModifiableEntity : ModifiableEntity<int>, IAuditable
{ }
public abstract class CreatableEntity : CreatableEntity<int>
{ }
public abstract class Entity : Entity<int>
{ }

public abstract class SoftDeletableEntity<TPrimaryKey> : CreatableEntity<TPrimaryKey>, ISoftDeletable
{
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public bool IsDeleted { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public string DeletionReason { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public Guid? DeleterUserId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public DateTime? DeletionTime { get; set; }
}

public abstract class FullAuditableEntity<TPrimaryKey> : ModifiableEntity<TPrimaryKey>, IFullAuditableEntity, ISoftDeletable
{
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public bool IsDeleted { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public string DeletionReason { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public Guid? DeleterUserId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public DateTime? DeletionTime { get; set; }
}

public abstract class ModifiableEntity<TPrimaryKey> : CreatableEntity<TPrimaryKey>, IModifiable
{
    public DateTime? LastModificationTime { get; set; }
    public Guid? LastModifierUserId { get; set; }
}

public abstract class CreatableEntity<TPrimaryKey> : Entity<TPrimaryKey>, ICreatable
{
    public DateTime CreationTime { get; set; }
    public Guid CreatorUserId { get; set; }
}

public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
{
    public TPrimaryKey Id { get; set; }
    public Guid UID { get; set; }
    public byte[] RowVersion { get; set; }
}