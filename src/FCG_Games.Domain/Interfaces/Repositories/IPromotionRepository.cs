using FCG_Games.Domain.Entities;

namespace FCG_Games.Domain.Interfaces.Repositories;

public interface IPromotionRepository : IBaseRepository<Promotion>
{
	Task<ICollection<Promotion>> GetAllActive();
}
