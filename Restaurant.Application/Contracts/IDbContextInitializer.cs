namespace Restaurant.Application.Contracts;

public interface IDbContextInitializer
{
	public Task InitializeAsync();
	public Task SeedAsync();
}

