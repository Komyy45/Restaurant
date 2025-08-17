using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurant.Domain.Entities.Common;

namespace Restaurant.Persistence.Data.Configurations.Common;

internal abstract class BaseEntityConfigurations<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
	where TEntity : BaseEntity<TKey>
	where TKey : IEquatable<TKey>
{
	public virtual void Configure(EntityTypeBuilder<TEntity> builder)
	{
		builder.Property(e => e.Id)
				.ValueGeneratedOnAdd();
	}
}

