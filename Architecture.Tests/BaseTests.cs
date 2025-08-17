using System.Reflection;
using Restaurant.Domain.Entities.Common;

namespace Architecture.Tests;

public abstract class BaseTests
{
     protected static readonly Assembly Domain = typeof(BaseEntity<>).Assembly;
     protected static readonly Assembly Application = typeof(Restaurant.Application.DependencyInjection).Assembly;
     protected static readonly Assembly API = typeof(Restaurant.API.DependencyInjection).Assembly;
     protected static readonly Assembly Persistence = typeof(Restaurant.Persistence.DependencyInjection).Assembly;
     protected static readonly Assembly Identity = typeof(Restaurant.Identity.DependencyInjection).Assembly;
     protected static readonly Assembly Infrastructure = typeof(Restaurant.Infrastructure.DependencyInjection).Assembly;
}
