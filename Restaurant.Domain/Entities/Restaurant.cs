using Restaurant.Domain.Entities.Common;

namespace Restaurant.Domain.Entities;
public sealed class Restaurant : BaseEntity<int>
{
	public string Name { get; set; } = default!;
	public string Description { get; set; } = default!;
	public string Category { get; set; } = default!;
	public bool HasDelivery { get; set; }
	public string? ContactEmail { get; set; } 
	public string? Contactumber { get; set; }
	public Address? Address { get; set; }
	public ICollection<Dish> Dishes { get;set; } = new HashSet<Dish>();
}
	
