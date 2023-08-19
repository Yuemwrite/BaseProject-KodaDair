using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration;

public class TransactionUserEntityConfiguration : IEntityTypeConfiguration<TransactionUser>
{
    public void Configure(EntityTypeBuilder<TransactionUser> builder)
    {
        builder.HasKey(_ => _.Id);

        builder.Property(_ => _.UserName);
        builder.Property(_=>_.Email).HasMaxLength(64).IsRequired();
        builder.Property(_ => _.Password).IsRequired();
        builder.Property(_ => _.MobilePhoneNumber).HasMaxLength(32).IsRequired();
    }
}