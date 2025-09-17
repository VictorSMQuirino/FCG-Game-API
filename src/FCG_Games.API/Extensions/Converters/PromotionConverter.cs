using FCG_Games.API.Requests.Promotion;
using FCG_Games.API.Responses.Promotion;
using FCG_Games.Domain.DTO.Promotion;

namespace FCG_Games.API.Extensions.Converters;

public static class PromotionConverter
{
	public static CreatePromotionDto ToDto(this CreatePromotionRequest request)
	   => new(request.GameId, request.DiscountPercentage, request.Deadline);

	public static UpdatePromotionDto ToDto(this UpdatePromotionRequest request)
		=> new(request.GameId, request.DiscountPercentage, request.Deadline);

	public static GetPromotionByIdResponse ToResponse(this PromotionDto dto)
		=> new(dto.Id, dto.Game, dto.DiscountPercentage, dto.Deadline, dto.Active);

	public static ICollection<GetPromotionByIdResponse> ToResponse(this ICollection<PromotionDto> dtoList)
		=> [.. dtoList.Select(ToResponse)];
}
