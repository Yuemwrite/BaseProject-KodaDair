using BaseProject.Infrastructure.Persistence.EntityConfiguration.Base;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration;

public class ProductEntityConfiguration : EntityTypeConfiguration<Product>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Product> builder, Type entityType)
    {
        builder.HasKey(_ => _.Id).IsClustered(); // clustered index.
        builder.Property(_ => _.Name);
        builder.HasIndex(_ => new { _.Name, _.Description }); //composite index
        builder.HasIndex(_ => _.Code).IsClustered(false).IsUnique();

        builder
            .HasOne(o => o.User)
            .WithMany(m => m.Products)
            .HasForeignKey(fk => fk.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}