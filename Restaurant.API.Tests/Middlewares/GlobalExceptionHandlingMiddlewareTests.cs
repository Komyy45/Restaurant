using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurant.API.Middlewares;
using Restaurant.Domain.Exceptions;

namespace Restaurant.API.Tests.Middlewares;

public class GlobalExceptionHandlingMiddlewareTests
{
    
    [Fact]
    public async Task InvokeAsync_WhenNoExceptionIsThrown_ShouldPassOutTheResponse()
    {
        var logger = new Mock<ILogger<GlobalExceptionHandlingMiddleware>>();
        var context = new DefaultHttpContext();
        var middleware = new GlobalExceptionHandlingMiddleware(logger.Object);
        var requestDelegateMock = new Mock<RequestDelegate>();
        
        await middleware.InvokeAsync(context,  requestDelegateMock.Object);
        
        requestDelegateMock.Verify(next => next.Invoke(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_WhenNotFoundExceptionIsThrown_ShouldReturn404NotFound()
    {
        var logger = new Mock<ILogger<GlobalExceptionHandlingMiddleware>>();
        var context = new DefaultHttpContext();
        var middleware = new GlobalExceptionHandlingMiddleware(logger.Object);
       

        await middleware.InvokeAsync(context, _ => throw new NotFoundException("1", "Entity"));
                
        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    } 
    [Fact]
    public async Task InvokeAsync_WhenNotUserDefinedExceptionIsThrown_ShouldReturnInternalServerError()
    {
        var logger = new Mock<ILogger<GlobalExceptionHandlingMiddleware>>();
        var context = new DefaultHttpContext();
        var middleware = new GlobalExceptionHandlingMiddleware(logger.Object);
       

        await middleware.InvokeAsync(context, _ => throw new Exception());
                
        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async Task InvokeAsync_WhenOperationForbiddenExceptionIsThrown_ShouldReturnForbidden()
    {
        var logger = new Mock<ILogger<GlobalExceptionHandlingMiddleware>>();
        var middleware = new GlobalExceptionHandlingMiddleware(logger.Object);
        var context = new DefaultHttpContext();
        var exception = new OperationForbiddenException();

        await middleware.InvokeAsync(context, _ => throw exception);

        context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }
    
    [Fact]
    public async Task InvokeAsync_WhenIsNotAllowedExceptionIsThrown_ShouldReturnForbidden()
    {
        var logger = new Mock<ILogger<GlobalExceptionHandlingMiddleware>>();
        var middleware = new GlobalExceptionHandlingMiddleware(logger.Object);
        var context = new DefaultHttpContext();
        var exception = new IsNotAllowedException();

        await middleware.InvokeAsync(context, _ => throw exception);

        context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }    
    
    [Fact]
    public async Task InvokeAsync_WhenAccountLockedExceptionIsThrown_ShouldReturnForbidden()
    {
        var logger = new Mock<ILogger<GlobalExceptionHandlingMiddleware>>();
        var middleware = new GlobalExceptionHandlingMiddleware(logger.Object);
        var context = new DefaultHttpContext();
        var exception = new AccountLockedException();

        await middleware.InvokeAsync(context, _ => throw exception);

        context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    } 
    
    [Fact]
    public async Task InvokeAsync_WhenValidationExceptionIsThrown_ShouldReturnBadRequest()
    {
        var logger = new Mock<ILogger<GlobalExceptionHandlingMiddleware>>();
        var middleware = new GlobalExceptionHandlingMiddleware(logger.Object);
        var context = new DefaultHttpContext();
        var exception = new ValidationException(new());

        await middleware.InvokeAsync(context, _ => throw exception);

        context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
    
    
}