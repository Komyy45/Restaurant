using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Persistence.Data.Configurations.Common;

namespace Restaurant.Persistence.Data.Configurations;
internal sealed class RestaurantConfigurations : BaseEntityConfigurations<Domain.Entities.Restaurant, int>
{
	public override void Configure(EntityTypeBuilder<Domain.Entities.Restaurant> builder)
	{
		base.Configure(builder);

		builder.OwnsOne(e => e.Address);

		builder.HasMany(e => e.Dishes)
				.WithOne()
				.HasForeignKey(e => e.RestaurantId);
	}
}

