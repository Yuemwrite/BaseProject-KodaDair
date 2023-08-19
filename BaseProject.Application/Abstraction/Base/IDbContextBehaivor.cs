using Shared.Enums;

namespace BaseProject.Application.Abstraction.Base;

public interface IDbContextBehaivor
{
    bool IsSoftDeleteFilterEnabled { get; }
    bool IsMerchantFilterEnabled { get; }
    bool IsCreationAuditingEnabled { get; }
    bool IsModificationAuditingEnabled { get; }
    bool IsDeletionAuditingEnabled { get; }

    bool IsEnabled(DbContextBehaviorItem behaviorItem);
    IDisposable Disable(DbContextBehaviorItem behaviorItem);
}