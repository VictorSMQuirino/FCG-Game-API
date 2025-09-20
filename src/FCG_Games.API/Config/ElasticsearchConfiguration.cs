using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace FCG_Games.API.Config;

public static class ElasticsearchConfiguration
{
	public static IServiceCollection AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
	{
		var settings = new ElasticsearchClientSettings(new Uri(configuration["Elasticsearch:Uri"]!))
			.Authentication(new BasicAuthentication(configuration["Elasticsearch:Username"]!, configuration["Elasticsearch:Password"]!))
			.ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true)
			.DefaultIndex(configuration["Elasticsearch:Index"]!);

		var client = new ElasticsearchClient(settings);

		services.AddSingleton(client);

		return services;
	}
}
