using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NetArchTest.Rules;
using Restaurant.API.Controllers.Common;

namespace Architecture.Tests.API;

public class APITests : BaseTests
{
    [Fact]
    public void Controllers_Should_EndWithController()
    {
        var result = Types.InAssembly(API)
            .That()
            .Inherit(typeof(BaseApiController))
            .Should()
            .HaveNameEndingWith("Controller")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Middlewares_Should_EndWithMiddleware()
    {
        var result = Types.InAssembly(API)
            .That()
            .ImplementInterface(typeof(IMiddleware))
            .Should()
            .HaveNameEndingWith("Middleware")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}