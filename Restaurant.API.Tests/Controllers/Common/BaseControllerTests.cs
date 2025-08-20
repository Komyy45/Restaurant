using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Restaurant.Domain.Common;
using Restaurant.Domain.Contracts;
using Restaurant.Domain.Entities.Common;

namespace Restaurant.API.Tests.Controllers.Common;

public abstract class BaseControllerTests<TEntity, TKey>(WebApplicationFactory<Program> webApplicationFactory) : IClassFixture<WebApplicationFactory<Program>>
where TEntity: BaseEntity<TKey>
where TKey: IEquatable<TKey>
{
    protected readonly Mock<IGenericRepository<TEntity, TKey>> _repositoryMock = new Mock<IGenericRepository<TEntity, TKey>>();
    
    protected WebApplicationFactory<Program> NonAuthorizedPolicyEvaluator() => ConfigureFactory(new ClaimsPrincipal());
    
    protected WebApplicationFactory<Program> OwnerPolicyEvaluator(string ownerId)
    {
        var claimsPrincipal = new ClaimsPrincipal();
        
        claimsPrincipal.AddIdentity(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, ownerId),
                new Claim(ClaimTypes.Role, RoleTypes.Owner),
                new Claim(ClaimTypes.Email, "test@gmail.com"), 
                new Claim(ClaimTypes.Name, "testUsername"), 
            ]
        ));
        
        return ConfigureFactory(claimsPrincipal);
    }
    
    protected WebApplicationFactory<Program> CustomerPolicyEvaluator()
    { 
        var claimsPrincipal = new ClaimsPrincipal();
        
        claimsPrincipal.AddIdentity(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, RoleTypes.Customer),
                new Claim(ClaimTypes.Email, "test@gmail.com"), 
                new Claim(ClaimTypes.Name, "testUsername"), 
            ]
        ));
        
        return ConfigureFactory(claimsPrincipal);
    }

    private WebApplicationFactory<Program> ConfigureFactory(ClaimsPrincipal claimsPrincipal)
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        unitOfWorkMock.Setup(e => e.GetRepository<TEntity, TKey>())
            .Returns(_repositoryMock.Object);
        
        return webApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator>(new FakePolicyEvaluator(claimsPrincipal));
                services.AddScoped<IUnitOfWork>(serviceProvider => unitOfWorkMock.Object);
            });
        });
    }
}