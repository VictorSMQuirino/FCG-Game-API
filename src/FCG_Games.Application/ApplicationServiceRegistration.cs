using FCG_Games.Application.Services;
using FCG_Games.Domain.Interfaces.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FCG_Games.Application;

public static class ApplicationServiceRegistration
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		services.AddScoped<IGameService, GameService>();
		services.AddScoped<IPromotionService, PromotionService>();

		return services;
	}
}
