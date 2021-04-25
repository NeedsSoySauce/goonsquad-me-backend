using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NeedsSoySauce.Data;
using NeedsSoySauce.Entities;

namespace NeedsSoySauce.Repositories
{
    public class GoonsquadsRepo : IGoonsquadsRepo
    {
        private ApplicationDbContext _context;
        private IGoonsRepo _goons;

        public GoonsquadsRepo(ApplicationDbContext context, IGoonsRepo goons)
        {
            _context = context;
            _goons = goons;
        }

        public Goonsquad CreateGoonsquad(string name, IEnumerable<Goon> goons)
        {
            Goonsquad squad = new Goonsquad(goons.ToList(), name);
            _context.Goonsquads.Add(squad);
            _context.SaveChanges();
            return squad;
        }

        public PagedResult<Goonsquad> GetGoonsquadsForGoon(string goonId, int page, int pageSize)
        {
            return _context.Goonsquads.OrderBy(g => g.CreatedOnUtc).Paginate(page, pageSize);
        }

        public List<Guid> GetGoonsquadIdsForGoon(string goonId)
        {
            return _context.Goonsquads.Select(s => s.Id).ToList();
        }

        public bool IsGoonMemberOfGoonsquad(string goonId, Guid goonsquadId)
        {
            // Should probably check if the goonsquad exists first
            var goonsquad = _context.Goonsquads.Include(s => s.Goons).Single(s => s.Id == goonsquadId);
            return goonsquad.Goons.Select(s => s.Id).Contains(goonId);
        }

        public Message CreateMessage(string content, Guid goonsquadId, string goonId)
        {
            var squad = _context.Goonsquads.Single(s => s.Id == goonsquadId);
            var goon = _goons.GetGoon(goonId) ?? throw new ArgumentNullException();
            Message message = new Message(content, squad, goon, goon.Username);
            _context.Messages.Add(message);
            _context.SaveChanges();
            return message;
        }

        public PagedResult<Message> GetMessagesForGoonsquad(Guid goonsquadId, int page, int pageSize)
        {
            return _context.Messages.OrderBy(m => m.SentOnUtc).Paginate(page, pageSize);
        }
    }
}