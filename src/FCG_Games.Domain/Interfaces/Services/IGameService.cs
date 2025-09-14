using FCG_Games.Domain.DTO;

namespace FCG_Games.Domain.Interfaces.Services;

public interface IGameService
{
	Task<Guid> CreateAsync(CreateGameDto dto);
	Task UpdateAsync(Guid id, UpdateGameDto dto);
	Task DeleteAsync(Guid id);
	Task<GameDto?> GetByIdAsync(Guid id);
	Task<ICollection<GameDto>> GetAllAsync();
}
