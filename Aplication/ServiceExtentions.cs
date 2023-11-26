using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;
public static class DependencyInjection
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        //services.AddAutoMapper(Assembly.GetExecutingAssembly());
        //services.AddMediatR(Assembly.GetExecutingAssembly());
        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}
