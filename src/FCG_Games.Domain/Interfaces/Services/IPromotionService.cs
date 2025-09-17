using FCG_Games.Domain.DTO.Promotion;

namespace FCG_Games.Domain.Interfaces.Services;

public interface IPromotionService
{
	Task<Guid> CreateAsync(CreatePromotionDto dto);
	Task UpdateAsync(Guid id, UpdatePromotionDto dto);
	Task<PromotionDto?> GetByIdAsync(Guid id);
	Task<ICollection<PromotionDto>> GetAllAsync();
	Task<ICollection<PromotionDto>> GetAllActiveAsync();
	Task ActivePromotionAsync(Guid id);
	Task DeactivePromotionAsync(Guid id);
}
