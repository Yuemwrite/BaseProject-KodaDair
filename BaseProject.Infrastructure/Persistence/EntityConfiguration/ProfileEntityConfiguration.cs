using BaseProject.Infrastructure.Persistence.EntityConfiguration.Base;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration;

public class ProfileEntityConfiguration : EntityTypeConfiguration<Profile>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Profile> builder, Type entityType)
    {
        builder.Property(_ => _.UserId).IsRequired();
        builder.Property(_ => _.Biography).IsRequired(false).HasMaxLength(1024);
        builder.Property(_ => _.Title).IsRequired().HasMaxLength(64);
        builder.Property(_ => _.SocialMediaAddress1).IsRequired(false).HasMaxLength(64);
        builder.Property(_ => _.SocialMediaAddress2).IsRequired(false).HasMaxLength(64);
        builder.Property(_ => _.WebSite).IsRequired(false).HasMaxLength(64);
        
        builder
            .HasOne(o => o.User)
            .WithMany(m => m.Profiles)
            .HasForeignKey(fk => fk.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        
    }
}