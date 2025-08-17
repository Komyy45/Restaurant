using FluentAssertions;
using NetArchTest.Rules;
using Restaurant.Domain.Entities.Common;
using Restaurant.Domain.Specifications;

namespace Architecture.Tests.Core;

public class DomainTests : BaseTests
{
    [Fact]
    public void Entities_Should_BeSealed()
    {
        var result = Types.InAssembly(Domain)
            .That()
            .Inherit(typeof(BaseEntity<>))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Exceptions_Should_BeEndedWithException()
    {
        var result = Types.InAssembly(Domain)
            .That()
            .Inherit(typeof(Exception))
            .Should()
            .HaveNameEndingWith("Exception")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Specification_Should_BeEndedBySpecification()
    {
        var result = Types.InAssembly(Domain)
            .That()
            .Inherit(typeof(BaseSpecification<,>))
            .Should()
            .HaveNameEndingWith("Specification")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}