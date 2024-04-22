using AutoMapper;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PristineCraft.Application.Common.Middleware;
using PristineCraft.Application.Extensions;
using PristineCraft.Application.Helper;
using PristineCraft.Domain.Entities.User;
using System.Runtime.InteropServices;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var config = new MapperConfiguration(cfg => { cfg.AddMaps(typeof(AutoMapperProfiles).Assembly); });
        var mapper = config.CreateMapper();

        services.AddAutoMapper(assemblies: typeof(AutoMapperProfiles).Assembly);
        services.AddControllers();
        services.AddFluentEmail(configuration["EmailSettings:DefaultFromEmail"]);
        services.AddSwagger(configuration);
        services.CreateIdentityServices(configuration);
        return services;
    }

    public static async Task<WebApplication> AddInfrastructureServices(this WebApplication app, IServiceScope scope, bool seeds)
    {
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        if (seeds)
            await Infrastructure.Helper.Seeds.SeederStartUpVerification(
                            scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>(),
                            context,
                            scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>());

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
        return app;
    }
}