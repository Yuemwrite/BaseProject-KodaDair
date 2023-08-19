using System.Diagnostics;
using System.Linq.Expressions;
using Domain.Base;
using Domain.Base.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration.Base;

public abstract class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    protected abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder, Type entityType);
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        var entityType = typeof(TEntity);

        #region PrimaryKey and Base entity properties
        ConfigureEntityKey(builder, entityType);
        #endregion

        #region Custom properties
        ConfigureEntity(builder, entityType);
        #endregion

        #region IApprovalable
        ConfigureIApprovalableProperties(builder, entityType);
        #endregion

        #region Auiditable properties
        ConfigureIModifiableProperties(builder, entityType);
        ConfigureICreatableProperties(builder, entityType);
        ConfigureISoftDeleteProperties(builder, entityType);
        ConfigureIPassivableProperties(builder, entityType);
        #endregion

        #region UID and RowVersion properties
        ConfigureEntityRowVersionAndUID(builder, entityType);
        #endregion
    }
    protected void AttachToDebugger()
    {
        if (!Debugger.IsAttached)
            Debugger.Launch();
    }
    protected virtual void ConfigureIApprovalableProperties(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        // Do nothing here, this class does not handle Approval entities.
    }

    protected virtual void ConfigureISoftDeleteProperties(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        if (!typeof(ISoftDeletable).IsAssignableFrom(entityType))
            return;

        builder
            .Property(nameof(ISoftDeletable.IsDeleted))
            .IsRequired();
        builder
            .Property(nameof(ISoftDeletable.DeletionReason))
            .IsRequired(false).HasMaxLength(2000);
        builder
            .Property(nameof(ISoftDeletable.DeletionTime))
            .IsRequired(false);
        builder
            .Property(nameof(ISoftDeletable.DeleterUserId))
            .IsRequired(false);
    }
    protected virtual void ConfigureICreatableProperties(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        if (!typeof(ICreatable).IsAssignableFrom(entityType))
            return;

        builder
            .Property(nameof(ICreatable.CreationTime))
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        builder
            .Property(nameof(ICreatable.CreatorUserId))
            .IsRequired()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
    }
    protected virtual void ConfigureIModifiableProperties(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        if (!typeof(IModifiable).IsAssignableFrom(entityType))
            return;

        builder
            .Property(nameof(IModifiable.LastModificationTime))
            .IsRequired(false)
            .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
        builder
            .Property(nameof(IModifiable.LastModifierUserId))
            .IsRequired(false)
            .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
    }

    protected virtual void ConfigureIPassivableProperties(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        if (!typeof(IPassivable).IsAssignableFrom(entityType))
            return;

        builder
            .Property(nameof(IPassivable.IsActive))
            .IsRequired();
    }
    protected virtual void ConfigureEntityKey(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        EntityTypeConfiguration<TEntity>.CheckEntityCompatibility(entityType);

        builder.HasKey(nameof(IEntity<int>.Id));
    }

    protected virtual void ConfigureEntityRowVersionAndUID(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        EntityTypeConfiguration<TEntity>.CheckEntityCompatibility(entityType);

        builder.Property(nameof(IEntity<int>.UID))
            .IsRequired()
            .HasDefaultValueSql("NEWID()")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        builder.Property(nameof(IEntity<int>.RowVersion))
            .IsRowVersion()
            .IsRequired();
    }
    
    protected virtual IndexBuilder AddFilteredUniqueIndex(EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, object>> indexExpression, string filter)
    {
        return AddUniqueIndex(builder, indexExpression).HasFilter(filter);
    }

    protected virtual IndexBuilder AddUniqueIndex(EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, object>> indexExpression)
    {
        return AddIndex(builder, indexExpression).IsUnique();
    }

    protected virtual IndexBuilder AddIndex(EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, object>> indexExpression)
    {
        return builder.HasIndex(indexExpression);
    }

    private static void CheckEntityCompatibility(Type entityType)
    {
        if (!typeof(IEntity<int>).IsAssignableFrom(entityType)
            && !typeof(IEntity<long>).IsAssignableFrom(entityType)
            && !typeof(IEntity<Guid>).IsAssignableFrom(entityType))
            throw new NotImplementedException($"Configuration for the entity type '{entityType.Name}' is not implemented. Please implement the Key configuration first by overriding the ConfigureEntityRowVersionAndUID!");
    }
}