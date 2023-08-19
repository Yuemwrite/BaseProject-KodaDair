using BaseProject.Infrastructure.Persistence.EntityConfiguration.Base;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration;

public class SavedPostEntityConfiguration : EntityTypeConfiguration<SavedPost>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SavedPost> builder, Type entityType)
    {
        builder.Property(_ => _.UserId).IsRequired();
        builder.Property(_ => _.SharingId).IsRequired();
        
        builder
            .HasOne(o => o.User)
            .WithMany(m => m.SavedPosts)
            .HasForeignKey(fk => fk.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(o => o.Sharing)
            .WithMany(m => m.SavedPosts)
            .HasForeignKey(fk => fk.SharingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}