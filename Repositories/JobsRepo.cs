using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NeedsSoySauce.Data;
using NeedsSoySauce.Entities;

namespace NeedsSoySauce.Repositories
{
    public class JobsRepo : IJobsRepo
    {
        private IGoonsRepo _goons;
        private ApplicationDbContext _context;

        public JobsRepo(ApplicationDbContext context, IGoonsRepo goons)
        {
            _context = context;
            _goons = goons;
        }

        public Job CreateJob(string goonId)
        {
            _goons.RecordGoon(goonId);
            var goon = _goons.GetGoon(goonId) ?? throw new ArgumentNullException($"No goon found with the id {goonId}");

            Job job = new Job(goon);
            _context.Jobs.Add(job);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return _context.Jobs.Single(j => j.GoonId == goonId);
            }

            return job;
        }

        public void RemoveJob(Guid jobId)
        {
            var job = _context.Jobs.Single(j => j.Id == jobId);
            _context.Jobs.Remove(job);
            _context.SaveChanges();
        }
    }
}