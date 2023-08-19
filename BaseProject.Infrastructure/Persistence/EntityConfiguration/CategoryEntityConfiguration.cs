using BaseProject.Infrastructure.Persistence.EntityConfiguration.Base;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration;

public class CategoryEntityConfiguration : EntityTypeConfiguration<Category>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Category> builder, Type entityType)
    {
        builder.Property(_ => _.Name).HasMaxLength(128);
    }
}