using FCG_Games.API.Extensions.Converters;
using FCG_Games.API.Requests.Promotion;
using FCG_Games.API.Responses.Promotion;
using FCG_Games.Domain.Enums;
using FCG_Games.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG_Games.API.Controllers;

[Authorize(Roles = nameof(UserRole.Admin))]
[ApiController]
[Route("api/v1/promotions")]
public class PromotionController : ControllerBase
{
	private readonly IPromotionService _promotionService;

	public PromotionController(IPromotionService promotionService)
	{
		_promotionService = promotionService;
	}

	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreatePromotionRequest request)
	{
		var dto = request.ToDto();

		var id = await _promotionService.CreateAsync(dto);

		return CreatedAtAction(nameof(GetById), new { id = id }, id);
	}

	[HttpPut("{id:guid}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdatePromotionRequest request)
	{
		var dto = request.ToDto();

		await _promotionService.UpdateAsync(id, dto);

		return NoContent();
	}

	[HttpGet("{id:guid}")]
	public async Task<ActionResult<GetPromotionByIdResponse>> GetById(Guid id)
	{
		var dto = await _promotionService.GetByIdAsync(id);

		return Ok(dto.ToResponse());
	}

	[HttpGet]
	public async Task<ActionResult<ICollection<GetPromotionByIdResponse>>> GetAll()
	{
		var dtoList = await _promotionService.GetAllAsync();

		return Ok(dtoList.ToResponse());
	}

	[HttpGet("active")]
	public async Task<ActionResult<ICollection<GetPromotionByIdResponse>>> GetAllActive()
	{
		var dtoList = await _promotionService.GetAllActiveAsync();

		return Ok(dtoList.ToResponse());
	}

	[HttpPatch("{id:guid}/active")]
	public async Task<ActionResult> Active(Guid id)
	{
		await _promotionService.ActivePromotionAsync(id);

		return NoContent();
	}

	[HttpPatch("{id:guid}/deactive")]
	public async Task<ActionResult> Deactive(Guid id)
	{
		await _promotionService.DeactivePromotionAsync(id);

		return NoContent();
	}
}
