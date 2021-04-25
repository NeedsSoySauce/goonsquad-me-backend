using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NeedsSoySauce.Authorization;
using NeedsSoySauce.Data;
using NeedsSoySauce.Entities;
using NeedsSoySauce.Repositories;

namespace NeedsSoySauce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoonsquadsController : ControllerBase
    {
        private readonly ILogger<GoonsquadsController> _logger;
        private IGoonsquadsRepo _repo;

        public GoonsquadsController(IGoonsquadsRepo repo, ILogger<GoonsquadsController> logger)
        {
            _logger = logger;
            _repo = repo;
        }

        [Authorize]
        [HttpGet]
        public PagedResult<Goonsquad> GetGoonsquads(int page = 1, int pageSize = 10)
        {
            return _repo.GetGoonsquadsForGoon(User.GetUserId(), page, pageSize);
        }

        [Authorize]
        [HttpGet("{id}/messages")]
        public PagedResult<Message> GetMessagesForGoonSquad(Guid id, int page = 1, int pageSize = 100)
        {
            return _repo.GetMessagesForGoonsquad(id, page, pageSize);
        }
    }
}
