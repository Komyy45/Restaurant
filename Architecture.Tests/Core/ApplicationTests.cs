using FluentAssertions;
using FluentValidation;
using MediatR;
using NetArchTest.Rules;
using Restaurant.Application.Common.messaging;
using Xunit.Abstractions;

namespace Architecture.Tests.Core;

public sealed class ApplicationTests : BaseTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ApplicationTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Handlers_Should_EndWithHandler()
    {
        var result = Types.InAssembly(Application)
            .That()
            .AreNotInterfaces()
            .And()
            .ImplementInterface(typeof(IRequestHandler<,>))
            .Or()
            .ImplementInterface(typeof(IRequestHandler<>))
            .Should()
            .HaveNameEndingWith("Handler")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Commands_Should_EndWithCommand()
    {
        var result = Types.InAssembly(Application)
            .That()
            .ImplementInterface(typeof(ICommand))
            .Should()
            .HaveNameEndingWith("Command")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Queries_Should_EndWithQuery()
    {
        var result = Types.InAssembly(Application)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should()
            .HaveNameEndingWith("Query")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Handlers_Should_BeSealed()
    {
        var result = Types.InAssembly(Application)
            .That()
            .AreNotInterfaces()
            .And()
            .ImplementInterface(typeof(IRequestHandler<,>))
            .Or()
            .ImplementInterface(typeof(IRequestHandler<>))
            .Should()
            .BeSealed()
            .GetResult();
        
        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Behaviors_Should_BeSealed()
    {
        var result = Types.InAssembly(Application)
            .That()
            .AreNotInterfaces()
            .And()
            .ImplementInterface(typeof(IPipelineBehavior<,>))
            .Should()
            .BeSealed()
            .GetResult();
        
        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Validators_Should_EndWithValidator()
    {
        var result = Types.InAssembly(Application)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .And()
            .AreNotAbstract()
            .Should()
            .HaveNameEndingWith("Validator")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Validators_Should_BeSealed()
    {
        var result = Types.InAssembly(Application)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
    
}