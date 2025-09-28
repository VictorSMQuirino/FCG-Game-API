using FCG_Games.Domain.DTO;
using FCG_Games.Domain.DTO.Elasticsearch.ElasticsearchDocuments;
using FCG_Games.Domain.Entities;

namespace FCG_Games.Application.Converters;

public static class GameConverter
{
	public static Game ToEntity(this CreateGameDto dto)
		=> new()
		{
			Title = dto.Title,
			Price = dto.Price,
			Description = dto.Description,
			Developer = dto.Developer,
			Publisher = dto.Publisher,
			ReleaseDate = dto.ReleaseDate,
			Genres = [.. dto.Genres],
			UserGames = []
		};

	public static Game ToEntity(this UpdateGameDto dto, Game game)
	{
		game.Title = dto.Title;
		game.Description = dto.Description;
		game.Developer = dto.Developer;
		game.Publisher = dto.Publisher;
		game.Price = dto.Price;
		game.ReleaseDate = dto.ReleaseDate;
		game.Genres = [.. dto.Genres];

		return game;
	}

	public static GameDto ToDto(this Game game)
		=> new(game.Id, game.Title, game.Description, game.Developer, game.Publisher, game.Price, game.ReleaseDate, game.Genres);

	public static List<GameDto> ToDtoList(this IEnumerable<Game> games)
		=> [.. games.Select(g => g.ToDto())];

	public static GameDocument ToElasticsearchDocument(this Game game)
		=> new()
		{
			Id = game.Id,
			Title = game.Title,
			Description = game.Description,
			Developer = game.Developer,
			Publisher = game.Publisher,
			ReleaseDate = game.ReleaseDate,
			Genres = [.. game.Genres.Select(g => g.ToString())]
		};
}
