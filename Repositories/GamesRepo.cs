using NeedsSoySauce.Data;
using NeedsSoySauce.Models;
using NeedsSoySauce.Services;

namespace NeedsSoySauce.Repositories
{
    public class GamesRepo : IGamesRepo
    {

        private IGamesService _gamesService;

        public GamesRepo(IGamesService gamesService)
        {
            _gamesService = gamesService;
        }

        public PagedResult<Game> GetGames(int page, int pageSize)
        {
            return _gamesService.GetGames(page, pageSize);
        }
    }
}