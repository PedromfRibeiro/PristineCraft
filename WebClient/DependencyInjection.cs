using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using WebClient;

namespace WebClient.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddRazorComponents().AddInteractiveServerComponents();
        services.AddFluentUIComponents();
        return services;
    }

    public static WebApplication UsePresentation(this WebApplication application)
    {
        application.MapRazorComponents<App>().AddInteractiveServerRenderMode();
        return application;
    }
}