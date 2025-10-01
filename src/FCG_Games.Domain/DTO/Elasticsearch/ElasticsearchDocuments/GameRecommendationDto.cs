namespace FCG_Games.Domain.DTO.Elasticsearch.ElasticsearchDocuments;

public record GameRecommendationDto(
	Guid Id,
	string Title,
	string? Description,
	string Developer,
	string Publisher,
	DateOnly ReleaseDate,
	ICollection<string> Genres
	);
