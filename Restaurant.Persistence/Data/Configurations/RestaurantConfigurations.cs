using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Persistence.Data.Configurations.Common;

namespace Restaurant.Persistence.Data.Configurations;
internal sealed class RestaurantConfigurations : BaseEntityConfigurations<Domain.Entities.Restaurant, int>
{
	public override void Configure(EntityTypeBuilder<Domain.Entities.Restaurant> builder)
	{
		base.Configure(builder);

		builder.Property(r => r.Name)
			.HasMaxLength(50)
			.IsRequired();

		builder.Property(r => r.Description)
			.HasMaxLength(300)
			.IsRequired();
		
		builder.OwnsOne(e => e.Address);

		builder.HasMany(e => e.Dishes)
				.WithOne()
				.HasForeignKey(e => e.RestaurantId);
	}
}

