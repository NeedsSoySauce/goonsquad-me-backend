using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NeedsSoySauce.Data;
using NeedsSoySauce.Models;
using NeedsSoySauce.Repositories;

namespace NeedsSoySauce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {

        private readonly ILogger<GamesController> _logger;
        private IGamesRepo _repo;

        public GamesController(IGamesRepo repo, ILogger<GamesController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public PagedResult<Game> Get(int page = 1, int pageSize = 10)
        {
            return _repo.GetGames(page, pageSize);
        }
    }
}
