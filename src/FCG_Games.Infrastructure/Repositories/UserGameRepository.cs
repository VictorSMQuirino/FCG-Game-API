using FCG_Games.Domain.Entities;
using FCG_Games.Domain.Interfaces.Repositories;
using FCG_Games.Infrastructure.Data;

namespace FCG_Games.Infrastructure.Repositories;

public class UserGameRepository(FCGGamesDbContext context) : BaseRepository<UserGame>(context), IUserGameRepository;
