using NeedsSoySauce.Data;
using NeedsSoySauce.Models;

namespace NeedsSoySauce.Repositories
{
    public interface IGamesRepo
    {
        PagedResult<Game> GetGames(int page, int pageSize);
    }
}