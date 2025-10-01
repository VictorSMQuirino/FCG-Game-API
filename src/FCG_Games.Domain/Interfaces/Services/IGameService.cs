using FCG_Games.Domain.DTO;
using FCG_Games.Domain.DTO.Elasticsearch;
using FCG_Games.Domain.DTO.Elasticsearch.ElasticsearchDocuments;

namespace FCG_Games.Domain.Interfaces.Services;

public interface IGameService
{
	Task<Guid> CreateAsync(CreateGameDto dto);
	Task UpdateAsync(Guid id, UpdateGameDto dto);
	Task DeleteAsync(Guid id);
	Task<GameDto?> GetByIdAsync(Guid id);
	Task<ICollection<GameDto>> GetAllAsync();
	Task<ICollection<GameDocument>> Search(ElasticsearchQueryParameters elasticsearchQueryParameters);
	Task<ICollection<GameRecommendationDto>> GetRecomendationsForUser(int topGenredCount = 2, int recommendationSize = 5);
	Task AddGameToUserLibrary(Guid gameId);
}
