using BaseProject.Infrastructure.Persistence.EntityConfiguration.Base;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration;

public class TwoFactorAuthenticationEntityConfiguration : EntityTypeConfiguration<TwoFactorAuthenticationTransaction>
{
    protected override void ConfigureEntity(EntityTypeBuilder<TwoFactorAuthenticationTransaction> builder, Type entityType)
    {
        builder.Property(_ => _.OneTimePasswordChannel).IsRequired();
        builder.Property(_ => _.OneTimePasswordType).IsRequired();
        builder.Property(_ => _.To).IsRequired().HasMaxLength(128);
        builder.Property(_ => _.OneTimePassword).HasMaxLength(1024).IsRequired();
        builder.Property(_ => _.UserId).IsRequired();
        builder.Property(_ => _.IsSend).IsRequired();
        builder.Property(_ => _.IsUsed).IsRequired();
        
        builder
            .HasOne(o => o.TransactionUser)
            .WithMany(m => m.TwoFactorAuthenticationTransactions)
            .HasForeignKey(fk => fk.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}