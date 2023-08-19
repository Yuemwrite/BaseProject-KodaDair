using BaseProject.Infrastructure.Persistence.EntityConfiguration.Base;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration;

public class SharingEntityConfiguration : EntityTypeConfiguration<Sharing>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Sharing> builder, Type entityType)
    {
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.UserId).IsRequired();
        builder.Property(_ => _.SharingType).IsRequired();
        builder.Property(_ => _.Title).IsRequired().HasMaxLength(128);
        builder.Property(_ => _.Content).IsRequired().HasMaxLength(2048);
        builder.Property(_ => _.CategoryId).IsRequired();
        builder.Property(_ => _.SubCategoryId).IsRequired(false);
        builder.Property(_ => _.IsFixed);

        builder
            .HasOne(o => o.User)
            .WithMany(m => m.Sharings)
            .HasForeignKey(fk => fk.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(o => o.Category)
            .WithMany(m => m.Sharings)
            .HasForeignKey(fk => fk.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(o => o.SubCategory)
            .WithMany(m => m.Sharings)
            .HasForeignKey(fk => fk.SubCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}