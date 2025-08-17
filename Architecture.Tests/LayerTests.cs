using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;

namespace Architecture.Tests;

public class LayerTests : BaseTests
{
    [Fact]
    public void Domain_Should_NotHaveAnyDependencies()
    {
        var result = Types.InAssembly(Domain)
            .Should()
            .NotHaveDependencyOnAny(
                Infrastructure.GetName().Name,
                Identity.GetName().Name,
                Persistence.GetName().Name,
                API.GetName().Name,
                Application.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Application_Should_DependOnlyOnDomain()
    {
        var result = Types.InAssembly(Domain)
            .Should()
            .NotHaveDependencyOnAny(
                Infrastructure.GetName().Name,
                Identity.GetName().Name,
                Persistence.GetName().Name,
                API.GetName().Name)
            .GetResult();
        
        result.IsSuccessful.Should().BeTrue();
    }
    [Fact]
    public void Identity_Should_DependOnlyOnApplication()
    {
        var result = Types.InAssembly(Domain)
            .Should()
            .NotHaveDependencyOnAny(
                Infrastructure.GetName().Name,
                Persistence.GetName().Name,
                API.GetName().Name)
            .GetResult();
        
        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Infrastructure_Should_DependOnlyOnApplication()
    {
        var result = Types.InAssembly(Domain)
            .Should()
            .NotHaveDependencyOnAny(
                Identity.GetName().Name,
                Persistence.GetName().Name,
                API.GetName().Name)
            .GetResult();
        
        result.IsSuccessful.Should().BeTrue();
    }   
    
    [Fact]
    public void Persistence_Should_DependOnlyOnApplication()
    {
        var result = Types.InAssembly(Domain)
                    .Should()
                    .NotHaveDependencyOnAny(
                        Infrastructure.GetName().Name,
                        Identity.GetName().Name,
                        API.GetName().Name)
                    .GetResult();
        
        result.IsSuccessful.Should().BeTrue();
    }
}