using NeedsSoySauce.Data;
using NeedsSoySauce.Models;

namespace NeedsSoySauce.Services
{
    public interface IGamesService
    {
        PagedResult<Game> GetGames(int page, int pageSize);
    }
}