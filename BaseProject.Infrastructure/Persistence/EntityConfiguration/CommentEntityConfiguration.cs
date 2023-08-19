using BaseProject.Infrastructure.Persistence.EntityConfiguration.Base;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration;

public class CommentEntityConfiguration : EntityTypeConfiguration<Comment>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Comment> builder, Type entityType)
    {
        builder.Property(_ => _.UserId).IsRequired();
        builder.Property(_ => _.SharingId).IsRequired();
        builder.Property(_ => _.Content).IsRequired().HasMaxLength(1024);
        
        builder
            .HasOne(o => o.User)
            .WithMany(m => m.Comments)
            .HasForeignKey(fk => fk.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(o => o.Sharing)
            .WithMany(m => m.Comments)
            .HasForeignKey(fk => fk.SharingId)
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}