using System;
using System.Collections.Generic;
using System.Linq;
using NeedsSoySauce.Data;
using NeedsSoySauce.Entities;
using NeedsSoySauce.Services;

namespace NeedsSoySauce.Repositories
{
    public class GoonsRepo : IGoonsRepo
    {
        private ApplicationDbContext _context;
        private Auth0ApiWrapper _auth0;

        public GoonsRepo(ApplicationDbContext context, Auth0ApiWrapper auth0)
        {
            _context = context;
            _auth0 = auth0;
        }

        public Goon? GetGoon(string goonId)
        {
            return _context.Goons.Single(g => g.Id == goonId);
        }

        public void RecordGoon(string id)
        {
            if (_context.Goons.Any(g => g.Id == id)) return;

            var user = _auth0.GetUser(id);

            var goon = new Goon(user.UserId, user.Username, new List<Goonsquad>());
            _context.Goons.Add(goon);
            _context.SaveChanges();
        }

        public void UpdateLastSeenOnUtc(string goonId)
        {
            var goon = _context.Goons.Single(g => g.Id == goonId);
            goon.LastSeenOnUtc = DateTime.UtcNow;
            _context.SaveChanges();
        }
    }
}