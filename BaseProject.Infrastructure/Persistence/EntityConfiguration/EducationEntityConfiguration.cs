using BaseProject.Infrastructure.Persistence.EntityConfiguration.Base;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration;

public class EducationEntityConfiguration : EntityTypeConfiguration<Education>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Education> builder, Type entityType)
    {
        builder.Property(_ => _.SchoolName).IsRequired().HasMaxLength(256);
        builder.Property(_ => _.EducationLevel).IsRequired();
        builder.Property(_=>_.StartDate).IsRequired();
        builder.Property(_=>_.EndDate).IsRequired(false);
        builder.Property(_ => _.UserId).IsRequired();

        builder
            .HasOne(o => o.User)
            .WithMany(m => m.Educations)
            .HasForeignKey(fk => fk.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}