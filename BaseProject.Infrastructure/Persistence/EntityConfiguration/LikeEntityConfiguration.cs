using BaseProject.Infrastructure.Persistence.EntityConfiguration.Base;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration;

public class LikeEntityConfiguration : EntityTypeConfiguration<Like>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Like> builder, Type entityType)
    {
        builder.Property(_ => _.SharingId).IsRequired(false);
        builder.Property(_ => _.CommentId).IsRequired(false);
        builder.Property(_ => _.ReplyId).IsRequired(false);
        
        builder
            .HasOne(o => o.Sharing)
            .WithMany(m => m.Likes)
            .HasForeignKey(fk => fk.SharingId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(o => o.Comment)
            .WithMany(m => m.Likes)
            .HasForeignKey(fk => fk.CommentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(o => o.Reply)
            .WithMany(m => m.Likes)
            .HasForeignKey(fk => fk.ReplyId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(o => o.User)
            .WithMany(m => m.Likes)
            .HasForeignKey(fk => fk.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}