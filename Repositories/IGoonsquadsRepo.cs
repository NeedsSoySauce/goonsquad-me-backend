using System;
using System.Collections.Generic;
using NeedsSoySauce.Data;
using NeedsSoySauce.Entities;

namespace NeedsSoySauce.Repositories
{
    public interface IGoonsquadsRepo
    {
        Goonsquad CreateGoonsquad(string name, IEnumerable<Goon> goons);

        PagedResult<Goonsquad> GetGoonsquadsForGoon(string goonId, int page, int pageSize);

        List<Guid> GetGoonsquadIdsForGoon(string goonId);

        bool IsGoonMemberOfGoonsquad(string goonId, Guid goonsquadId);

        Message CreateMessage(string message, Guid goonsquadId, string goonId);

        PagedResult<Message> GetMessagesForGoonsquad(Guid goonsquadId, int page, int pageSize);
    }
}