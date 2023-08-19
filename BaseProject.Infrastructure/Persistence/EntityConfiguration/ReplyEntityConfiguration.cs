using BaseProject.Infrastructure.Persistence.EntityConfiguration.Base;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration;

public class ReplyEntityConfiguration : EntityTypeConfiguration<Reply>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Reply> builder, Type entityType)
    {
        builder.Property(_ => _.UserId).IsRequired();
        builder.Property(_ => _.CommentId).IsRequired();
        builder.Property(_ => _.Content).IsRequired().HasMaxLength(1024);
        
        
        builder
            .HasOne(o => o.User)
            .WithMany(m => m.Replies)
            .HasForeignKey(fk => fk.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(o => o.Comment)
            .WithMany(m => m.Replies)
            .HasForeignKey(fk => fk.CommentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}