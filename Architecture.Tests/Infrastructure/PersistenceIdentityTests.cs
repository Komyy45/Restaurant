using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetArchTest.Rules;

namespace Architecture.Tests.Infrastructure;

public class PersistenceIdentityTests : BaseTests
{
    [Fact]
    public void Configurations_Should_EndWithConfigurations()
    {
        var result = Types.InAssemblies([Persistence, Identity])
            .That()
            .AreNotAbstract()
            .And()
            .ImplementInterface(typeof(IEntityTypeConfiguration<>))
            .Should()
            .HaveNameEndingWith("Configurations")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void DbContexts_Should_EndWithDbContext()
    {
        var result = Types.InAssemblies([Persistence, Identity])
            .That()
            .Inherit(typeof(DbContext))
            .Should()
            .HaveNameEndingWith("DbContext")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}