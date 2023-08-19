using BaseProject.Infrastructure.Persistence.EntityConfiguration.Base;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration;

public class FollowerEntityConfiguration : EntityTypeConfiguration<Follower>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Follower> builder, Type entityType)
    {
        builder.Property(_ => _.FollowerUserId);
        builder.Property(_ => _.FollowedUserId);
        builder.Property(_ => _.ApprovalStatus).IsRequired();

        #region RelationConfiguration

        builder
            .HasOne(o => o.FollowerUser)
            .WithMany(m => m.FollowerUsers)
            .HasForeignKey(fk => fk.FollowerUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(o => o.FollowedUser)
            .WithMany(m => m.FollowedUsers)
            .HasForeignKey(fk => fk.FollowedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion
        
        
    }
}