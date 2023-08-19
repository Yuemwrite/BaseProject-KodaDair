using BaseProject.Application.Abstraction.Base;
using BaseProject.Application.Common.Exceptions;
using Shared.Common;
using Shared.Enums;

namespace BaseProject.Infrastructure.Context;

public class DbContextBehaivor : IDbContextBehaivor
{
    private DbContextBehaviorItem disabledBehaviorItems;


    public bool IsSoftDeleteFilterEnabled => IsEnabled(DbContextBehaviorItem.SoftDeleteFilter);
    public bool IsMerchantFilterEnabled => IsEnabled(DbContextBehaviorItem.MerchantFilter);
    public bool IsCreationAuditingEnabled => IsEnabled(DbContextBehaviorItem.CreationAuditing);
    public bool IsModificationAuditingEnabled => IsEnabled(DbContextBehaviorItem.ModificationAuditing);
    public bool IsDeletionAuditingEnabled  => IsEnabled(DbContextBehaviorItem.DeletionAuditing);
    public bool IsEnabled(DbContextBehaviorItem behaviorItem)
    {
        return (disabledBehaviorItems & behaviorItem) == DbContextBehaviorItem.None;
    }

    public IDisposable Disable(DbContextBehaviorItem behaviorItem)
    {
        if (behaviorItem == DbContextBehaviorItem.None)
            throw new CustomException("You must supply at least one behavior item to disable it!");

        var diff = disabledBehaviorItems & behaviorItem;

        if (diff == behaviorItem)
        {
            return new DisposeAction(() => Enable());
        }
        else if (diff == DbContextBehaviorItem.None)
        {
            disabledBehaviorItems |= behaviorItem;
            return new DisposeAction(() => Enable(behaviorItem));
        }

        var missingBehavior = behaviorItem & ~diff;
        disabledBehaviorItems |= missingBehavior;
        return new DisposeAction(() => Enable(missingBehavior));
    }
    
    private void Enable(DbContextBehaviorItem? behaviorItem = null)
    {
        if (!behaviorItem.HasValue || disabledBehaviorItems == DbContextBehaviorItem.None)
            return;

        disabledBehaviorItems &= ~behaviorItem.Value;
    }
}