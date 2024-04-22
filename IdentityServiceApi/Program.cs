using Infrastructure.Repositories;
using PristineCraft.Application.Common.Interfaces;
using PristineCraft.Application.Services;
using MediatR;
using PristineCraft.Application.Common.Behaviours;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddSingleton<PristineCraft.Application.Common.Resources.ResourceManager>();

builder.Services.AddInfrastructureServices(builder.Configuration);
var app = builder.Build();
using var scope = app.Services.CreateScope();
await app.AddInfrastructureServices(scope, true);