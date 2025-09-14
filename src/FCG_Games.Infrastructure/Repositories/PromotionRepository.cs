using FCG_Games.Domain.Entities;
using FCG_Games.Domain.Interfaces.Repositories;
using FCG_Games.Infrastructure.Data;

namespace FCG_Games.Infrastructure.Repositories;

public class PromotionRepository(FCGGamesDbContext context) : BaseRepository<Promotion>(context), IPromotionRepository
{
	public async Task<ICollection<Promotion>> GetAllActive()
		=> await GetListBy(p => p.Active, p => p.Game!);
}
