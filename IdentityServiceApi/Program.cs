using Application.Interfaces;
using Application.Services;
using Persistence.Repositories;
using Shared.Helper;
using Shared.Extensions;
using Shared.Middleware;
using Domain.Entities;
using Persistence;
using Persistence.DataSeeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Infrastructure.Services;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region Injection Interfaces

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

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
		var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
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