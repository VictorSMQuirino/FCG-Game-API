using FCG_Games.API.Extensions.Converters;
using FCG_Games.API.Requests.Game;
using FCG_Games.API.Requests.Payment;
using FCG_Games.API.Responses;
using FCG_Games.API.Responses.Game;
using FCG_Games.Domain.DTO;
using FCG_Games.Domain.DTO.Elasticsearch;
using FCG_Games.Domain.DTO.Elasticsearch.ElasticsearchDocuments;
using FCG_Games.Domain.Enums;
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

	[Authorize(Roles = nameof(UserRole.Admin))]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
	[HttpPost]
	public async Task<ActionResult> Create(CreateGameRequest request)
	{
		var dto = request.ToDto();

		var id = await _gameService.CreateAsync(dto);

		return CreatedAtAction(nameof(GetById), new { id }, id);
	}

	[Authorize(Roles = nameof(UserRole.Admin))]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdateGameRequest request)
	{
		var dto = request.ToDto();

		await _gameService.UpdateAsync(id, dto);

		return NoContent();
	}

	[Authorize(Roles = nameof(UserRole.Admin))]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		await _gameService.DeleteAsync(id);

		return NoContent();
	}

	[AllowAnonymous]
	[ProducesResponseType(typeof(GetGameByIdResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
	[HttpGet("{id:guid}")]
	public async Task<ActionResult<GetGameByIdResponse>> GetById(Guid id)
	{
		var dto = await _gameService.GetByIdAsync(id);

		return Ok(dto?.ToResponse());
	}

	[AllowAnonymous]
	[ProducesResponseType(typeof(ICollection<GetGameByIdResponse>), StatusCodes.Status200OK)]
	[HttpGet]
	public async Task<ActionResult<ICollection<GetGameByIdResponse>>> GetAll()
	{
		var dtoList = await _gameService.GetAllAsync();

		return Ok(dtoList.ToResponse());
	}

	[AllowAnonymous]
	[ProducesResponseType(typeof(ICollection<GameDocument>), StatusCodes.Status200OK)]
	[HttpGet("search")]
	public async Task<ActionResult<ICollection<GameDocument>>> SearchGames([FromQuery] ElasticsearchQueryParameters elasticsearchQueryParameters)
	{
		var gameDocuments = await _gameService.Search(elasticsearchQueryParameters);

		return Ok(gameDocuments);
	}

	[Authorize]
	[ProducesResponseType(typeof(ICollection<GameDocumentResponseDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[HttpGet("recommendations")]
	public async Task<ActionResult<ICollection<GameDocumentResponseDto>>> GetRecommendationsForUser([FromQuery]GetRecommendationsRequest request)
	{
		var recommendations = await _gameService.GetRecomendationsForUser(request.TopGenredCount ?? 2, request.RecommentetionSize ?? 5);

		return recommendations.Count > 0 ? Ok(recommendations) : NoContent();
	}

	[Authorize]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
	[HttpPost("add-game-to-user-library/{gameId:guid}")]
	public async Task<ActionResult> AddGameToUserLibrary(Guid gameId, PaymentInfo paymentInfo)
	{
		await _gameService.AddGameToUserLibrary(gameId, paymentInfo.PaymentMethod);

		return NoContent();
	}

	[AllowAnonymous]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
	[HttpPost("{gameId:guid}/guarante-access/{userId:guid}")]
	public async Task<ActionResult> GuaranteGameAccessToUser(Guid gameId, Guid userId)
	{
		await _gameService.GuaranteAccessToGameForUser(userId, gameId);

		return NoContent();
	}

	[Authorize]
	[ProducesResponseType(typeof(ICollection<GetGameByIdResponse>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[HttpGet("in-library-for-logged-user")]
	public async Task<ActionResult<GetGameByIdResponse>> GetGamesInLibrary()
	{
		var dtoList = await _gameService.GetGamesInLibraryOfLoggedUser();

		return dtoList.Count != 0 ? Ok(dtoList.ToResponse()) : NoContent();
	}
}
