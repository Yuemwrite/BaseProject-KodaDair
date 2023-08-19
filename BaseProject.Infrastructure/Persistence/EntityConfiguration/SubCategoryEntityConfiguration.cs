using BaseProject.Infrastructure.Persistence.EntityConfiguration.Base;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration;

public class SubCategoryEntityConfiguration : EntityTypeConfiguration<SubCategory>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SubCategory> builder, Type entityType)
    {
        builder.Property(_ => _.Name).IsRequired().HasMaxLength(128);
        builder.Property(_ => _.CategoryId).IsRequired();
        
        builder
            .HasOne(o => o.Category)
            .WithMany(m => m.SubCategories)
            .HasForeignKey(fk => fk.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(o => o.Category)
            .WithMany(m => m.SubCategories)
            .HasForeignKey(fk => fk.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}