using FCG_Games.API.Extensions.Converters;
using FCG_Games.API.Requests.Game;
using FCG_Games.API.Responses.Game;
using FCG_Games.Domain.DTO.Elasticsearch;
using FCG_Games.Domain.DTO.Elasticsearch.ElasticsearchDocuments;
using FCG_Games.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG_Games.API.Controllers;

[ApiController]
[Route("api/v1/games")]
public class GameController : ControllerBase
{
	private readonly IGameService _gameService;

	public GameController(IGameService gameService)
	{
		_gameService = gameService;
	}

	[HttpPost]
	public async Task<ActionResult> Create(CreateGameRequest request)
	{
		var dto = request.ToDto();

		var id = await _gameService.CreateAsync(dto);

		return CreatedAtAction(nameof(GetById), new { id }, id);
	}

	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdateGameRequest request)
	{
		var dto = request.ToDto();

		await _gameService.UpdateAsync(id, dto);

		return NoContent();
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		await _gameService.DeleteAsync(id);

		return NoContent();
	}

	[AllowAnonymous]
	[HttpGet("{id:guid}")]
	public async Task<ActionResult<GetGameByIdResponse>> GetById(Guid id)
	{
		var dto = await _gameService.GetByIdAsync(id);

		return Ok(dto?.ToResponse());
	}

	[AllowAnonymous]
	[HttpGet]
	public async Task<ActionResult<ICollection<GetGameByIdResponse>>> GetAll()
	{
		var dtoList = await _gameService.GetAllAsync();

		return Ok(dtoList.ToResponse());
	}

	[AllowAnonymous]
	[HttpGet("search")]
	public async Task<ActionResult<ICollection<GameDocument>>> SearchGames([FromQuery] ElasticsearchQueryParameters elasticsearchQueryParameters)
	{
		var gameDocuments = await _gameService.Search(elasticsearchQueryParameters);

		return Ok(gameDocuments);
	}
}
