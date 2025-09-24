using FCG_Games.API.Requests.Game;
using FCG_Games.API.Responses.Game;
using FCG_Games.Domain.DTO;

namespace FCG_Games.API.Extensions.Converters;

public static class GameConverter
{
	public static CreateGameDto ToDto(this CreateGameRequest request)
		=> new(request.Title, request.Description, request.Developer, request.Publisher, request.Price, request.ReleaseDate, request.Genres);

	public static UpdateGameDto ToDto(this UpdateGameRequest request)
		=> new(request.Title, request.Description, request.Developer, request.Publisher, request.Price, request.ReleaseDate, request.Genres);

	public static GetGameByIdResponse ToResponse(this GameDto dto)
		=> new(dto.Id, dto.Title, dto.Description, dto.Developer, dto.Publisher, dto.Price, dto.ReleaseDate, dto.Genres.Select(g => g.ToString()));

	public static ICollection<GetGameByIdResponse> ToResponse(this ICollection<GameDto> dtoList)
		=> [.. dtoList.Select(ToResponse)];
}
