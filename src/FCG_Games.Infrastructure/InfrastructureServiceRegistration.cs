using FCG_Games.Domain.Interfaces.Repositories;
using FCG_Games.Infrastructure.Data;
using FCG_Games.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FCG_Games.Infrastructure;

public static class InfrastructureServiceRegistration
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<FCGGamesDbContext>(options => options.UseNpgsql(
			configuration.GetConnectionString("DefaultConnection")
			));

		services.AddScoped<IGameRepository, GameRepository>();
		services.AddScoped<IPromotionRepository, PromotionRepository>();
		services.AddScoped<IUserGameRepository, UserGameRepository>();

		return services;
	}
}
