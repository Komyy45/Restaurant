namespace Restaurant.Application.Contracts;

public interface IDbContextInitializer
{
	public ValueTask InitializeAsync();
	public ValueTask SeedAsync();
}

