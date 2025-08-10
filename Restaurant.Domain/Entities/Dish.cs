using Restaurant.Domain.Entities.Common;

namespace Restaurant.Domain.Entities;
public sealed class Dish : BaseEntity<int>
{
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public decimal Price { get; set; }
	public int RestaurantId { get; set; }
}

