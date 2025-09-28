using FCG_Games.API.Extensions;
using FCG_Games.Domain.Interfaces.Services;

namespace FCG_Games.API;

public static class ApiServicesRegistration
{
	public static IServiceCollection AddApiServices(this IServiceCollection services)
	{
		services.AddScoped<IApplicationUserService, ApplicationUser>();

		return services;
	}
}
