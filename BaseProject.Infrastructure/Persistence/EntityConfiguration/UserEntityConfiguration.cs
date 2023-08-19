using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        #region Primary Key

        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Role).IsRequired();

        #endregion

        #region Columns

        builder.Property(_=>_.UserName).HasMaxLength(64).IsRequired();
        builder.Property(_=>_.Email).HasMaxLength(64).IsRequired();
        builder.Property(_=>_.MobilePhoneNumber).HasMaxLength(32).IsRequired();
        builder.Property(_ => _.Password).IsRequired();
        builder.Property(_ => _.ExpirationTime).IsRequired();
        builder.Property(_ => _.RefreshToken).IsRequired(false);
        builder.Property(_ => _.RefreshTokenTime);
        builder.Property(_ => _.IsPrivate);

        #endregion
    }
}