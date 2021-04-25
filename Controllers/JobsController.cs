using System;
using System.Collections.Generic;
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
    public class JobsController : ControllerBase
    {
        private readonly ILogger<JobsController> _logger;
        private IJobsRepo _repo;

        public JobsController(ILogger<JobsController> logger, IJobsRepo repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [Authorize]
        [HttpPost]
        public Job CreateJob()
        {
            var job = _repo.CreateJob(User.GetUserId());
            return job;
        }
    }
}
