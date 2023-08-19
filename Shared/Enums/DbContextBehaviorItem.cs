namespace Shared.Enums;

public enum DbContextBehaviorItem
{
    None = 0,
    SoftDeleteFilter = 1,
    MerchantFilter = 2,
    CreationAuditing = 4,
    ModificationAuditing = 8,
    DeletionAuditing = 16,
    All = SoftDeleteFilter | MerchantFilter | CreationAuditing | ModificationAuditing | DeletionAuditing
}