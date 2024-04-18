using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Persistence;
using Persistence.Repositories;
using Shared.Extensions;
using Shared.Helper;
using Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);

#region Injection Interfaces
builder.Services.AddTransient<IEventRepository, EventRepository>();
#endregion Injection Interfaces

var config = new MapperConfiguration(cfg => { cfg.AddMaps(typeof(AutoMapperProfiles).Assembly); });
var mapper = config.CreateMapper();

builder.Services.AddAutoMapper(assemblies: typeof(AutoMapperProfiles).Assembly);
builder.Services.AddControllers();
builder.Services.AddSwagger(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.CreateIdentityServices(builder.Configuration);

var app = builder.Build();
using var scope = app.Services.CreateScope();

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