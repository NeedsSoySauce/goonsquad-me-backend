using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NeedsSoySauce.Entities;
using NeedsSoySauce.Repositories;

namespace NeedsSoySauce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoonsController : ControllerBase
    {
        private readonly ILogger<GoonsController> _logger;
        private IGoonsRepo _repo;

        public GoonsController(IGoonsRepo repo, ILogger<GoonsController> logger)
        {
            _logger = logger;
            _repo = repo;
        }

        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Goon> Get(string id)
        {
            var goon = _repo.GetGoon(id);

            if (goon is null)
            {
                return NotFound();
            }

            return Ok(goon);
        }
    }
}
