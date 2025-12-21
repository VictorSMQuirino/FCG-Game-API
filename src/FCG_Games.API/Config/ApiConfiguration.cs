using FCG_Games.API.Consumers;
using FCG_Games.API.Middlewares;
using FCG_Games.Application.Auth;
using FCG_Games.Domain.External.Payments.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Refit;

namespace FCG_Games.API.Config;

public static class ApiConfiguration
{
	public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

		services.AddHttpContextAccessor();

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidIssuer = jwtSettings?.Issuer,
				ValidateAudience = true,
				ValidAudience = jwtSettings?.Audience,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings!.Key)),
				ValidateLifetime = true
			};
		});

		services.AddAuthorization();

		return services;
	}

	public static IServiceCollection ConfigureRefit(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddTransient<RefitLoggingHandler>();

		services
			.AddRefitClient<IPaymentsApi>()
			.ConfigureHttpClient(c =>
			{
				c.BaseAddress = new Uri(configuration["PaymentsApi:Url"]!);
			})
			.AddHttpMessageHandler<RefitLoggingHandler>();

		return services;
	}

	public static IServiceCollection ConfigureOpenTelemetry(this IServiceCollection services, string applicationName)
	{
		services.AddOpenTelemetry()
			.ConfigureResource(resource => resource.AddService(applicationName))
			.WithMetrics(metrics =>
			{
				metrics
				.AddAspNetCoreInstrumentation()
				.AddRuntimeInstrumentation()
				.AddPrometheusExporter();

			});

		return services;
	}

	public static IServiceCollection ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMassTransit(c =>
		{
			c.AddConsumer<PaymentSucceededConsumer>();

			c.UsingRabbitMq((context, cfg) =>
			{
				cfg.Host(configuration["RabbitMQ:Host"], h =>
				{
					h.Username(configuration["RabbitMQ:Username"]!);
					h.Password(configuration["RabbitMQ:Password"]!);
				});

				cfg.ConfigureEndpoints(context);
			});
		});

		return services;
	}
}
