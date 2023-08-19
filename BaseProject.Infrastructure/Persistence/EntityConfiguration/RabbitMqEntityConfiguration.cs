using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.EntityConfiguration;

public class RabbitMqEntityConfiguration : IEntityTypeConfiguration<RabbitMqInfo>
{
    public void Configure(EntityTypeBuilder<RabbitMqInfo> builder)
    {
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.Info);
    }
}