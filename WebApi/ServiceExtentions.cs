using Application.Interfaces;
using WebApi.Services;

namespace WebApi;

public static class ServiceExtentions
{
    public static IServiceCollection ConfigureWebApi(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();

        return services;
    }
}