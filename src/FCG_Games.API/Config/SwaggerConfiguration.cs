namespace FCG_Games.API.Config;

public static class SwaggerConfiguration
{
	public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
	{
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
			{
				Title = "FIAP Cloud Games API - Games Microservice",
				Version = "v1"
			});

			options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
			{
				Name = "Authorization",
				Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
				Scheme = "Bearer",
				BearerFormat = "JWT",
				In = Microsoft.OpenApi.Models.ParameterLocation.Header,
				Description = "Digite: {seu_token_jwt}"
			});

			options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
			{
				{
					new Microsoft.OpenApi.Models.OpenApiSecurityScheme
					{
						Reference = new Microsoft.OpenApi.Models.OpenApiReference
						{
							Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					Array.Empty<string>()
				}
			});
		});

		return services;
	}
}
