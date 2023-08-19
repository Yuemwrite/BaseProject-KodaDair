using BaseProject.Infrastructure.Persistence.EntityConfiguration.Base;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration;

public class WorkExperienceEntityConfiguration : EntityTypeConfiguration<WorkExpeirence>
{
    protected override void ConfigureEntity(EntityTypeBuilder<WorkExpeirence> builder, Type entityType)
    {
        builder.Property(_ => _.CompanyName).IsRequired().HasMaxLength(256);
        builder.Property(_ => _.Remark).IsRequired(false).HasMaxLength(512);
        builder.Property(_ => _.StartDate).IsRequired();
        builder.Property(_ => _.EndDate).IsRequired(false);
        builder.Property(_ => _.UserId).IsRequired();
        
        builder
            .HasOne(o => o.User)
            .WithMany(m => m.WorkExpeirences)
            .HasForeignKey(fk => fk.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}