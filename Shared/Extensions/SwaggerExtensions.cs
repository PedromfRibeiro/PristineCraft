using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Shared.Extensions;

public static class SwaggerExtensions
{
	public static void AddSwagger(this IServiceCollection services, IConfiguration _configuration)
	{
		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo
			{
				Version = _configuration?["SwaggerGen:OpenApiInfo:Version"],
				Title = _configuration?["SwaggerGen:OpenApiInfo:Title"],
				Description = _configuration?["SwaggerGen:OpenApiInfo:Description"],
				TermsOfService = new System.Uri(_configuration?["SwaggerGen:OpenApiInfo:TermsOfService"] ?? ""),
				Contact = new OpenApiContact()
				{
					Name = _configuration?["SwaggerGen:OpenApiInfo:Contact:Name"],
					Email = _configuration?["SwaggerGen:OpenApiInfo:Contact:Email"]
				}
			});
			c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Description = _configuration?["SwaggerGen:OpenApiInfo:SecurityDefinition:Description"],
				Name = "Authorization",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer"
			});
			c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						Array.Empty<string>()
					}
				});
		});
	}
}