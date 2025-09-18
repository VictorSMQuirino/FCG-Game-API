using FCG_Games.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FCG_Games.API.Config;

public static class SeedConfig
{
	public static async Task EnsureMigrationApplied(IServiceProvider serviceProvider)
	{
		using var scope = serviceProvider.CreateScope();
		var services = scope.ServiceProvider;
		var context = services.GetRequiredService<FCGGamesDbContext>();
		var enviroment = services.GetRequiredService<IWebHostEnvironment>();
		var logger = services.GetRequiredService<ILogger<FCGGamesDbContext>>();

		if (!enviroment.IsDevelopment())
		{
			try
			{
				logger.LogInformation("Verifying and applying database migrations...");
				await context.Database.MigrateAsync();
				logger.LogInformation("Database migrations successfully applied.");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An error occurred while applying database migrations.");
				throw;
			}
		}
		else
		{
			logger.LogInformation("Development environment detected. Skipping automatic database migration.");
		}
	}
}
