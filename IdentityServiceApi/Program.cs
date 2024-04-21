using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Helper;
using Infrastructure.Extensions;
using PristineCraft.Domain.Entities.User;
using PristineCraft.Application.Common.Interfaces;
using PristineCraft.Application.Services;
using PristineCraft.Application.Common.Middleware;
using PristineCraft.Application.Extensions;
using PristineCraft.Application.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Injection Interfaces

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddSingleton<PristineCraft.Application.Common.Resources.ResourceManager>();

#endregion Injection Interfaces

var config = new MapperConfiguration(cfg => { cfg.AddMaps(typeof(AutoMapperProfiles).Assembly); });
var mapper = config.CreateMapper();

builder.Services.AddAutoMapper(assemblies: typeof(AutoMapperProfiles).Assembly);
builder.Services.AddControllers();
builder.Services.AddFluentEmail(builder.Configuration["EmailSettings:DefaultFromEmail"]);
builder.Services.AddSwagger(builder.Configuration);
builder.Services.CreateIdentityServices(builder.Configuration);

var app = builder.Build();
using var scope = app.Services.CreateScope();

// only during development, validate your mappings; remove it before release
#if DEBUG
//config.AssertConfigurationIsValid();
#endif
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    #region Sync and seed DataBase

    try
    {
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();

        // Se houver uma nova migra��o:
        //		A DB � eliminada
        //		� iniciado o processo de migra��o
        //		E o seed da db come�a
        var migration = context.Database.GetPendingMigrations();
        if (true)//migration.Any())
        {
            context.Database.EnsureDeleted();

            context.Database.Migrate();

            await context.Database.MigrateAsync();
            await Seeds.SeederStartUpVerification(UserManager, context, RoleManager);
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred durring migration");
    }

    #endregion Sync and seed DataBase
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();