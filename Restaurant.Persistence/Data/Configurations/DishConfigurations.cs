using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities;
using Restaurant.Persistence.Data.Configurations.Common;

namespace Restaurant.Persistence.Data.Configurations;

internal sealed class DishConfigurations : BaseEntityConfigurations<Dish, int>
{
    public override void Configure(EntityTypeBuilder<Dish> builder)
    {
        base.Configure(builder);

        builder.Property(d => d.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.Description)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(d => d.Price)
            .HasColumnType("DECIMAL(18,2)")
            .IsRequired();
    }
}