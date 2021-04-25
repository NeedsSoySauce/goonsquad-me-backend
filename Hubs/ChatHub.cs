using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NeedsSoySauce.Authorization;
using NeedsSoySauce.Entities;
using NeedsSoySauce.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NeedsSoySauce.SignalR
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub
    {
        private IGoonsquadsRepo _goonsquads;
        private IGoonsRepo _goons;

        public ChatHub(IGoonsquadsRepo goonsquads, IGoonsRepo goons)
        {
            _goonsquads = goonsquads;
            _goons = goons;
        }

        private string GetUserId()
        {
            return Context.User?.GetUserId() ?? throw new ArgumentNullException(nameof(Context.User));
        }

        public async override Task OnConnectedAsync()
        {
            var userId = GetUserId();

            // If this is a new user, add them to our database to 'sync' it with Auth0
            _goons.RecordGoon(userId);

            // Add this goon to a group for each of the goonsquards they're a member of
            var ids = _goonsquads.GetGoonsquadIdsForGoon(userId);
            var tasks = ids.Select(id => Groups.AddToGroupAsync(Context.ConnectionId, id.ToString())).ToArray();
            await Clients.All.SendAsync("message", $"{userId} connected");
            await Task.WhenAll(tasks);
        }

        public Task Heartbeat()
        {
            var userId = GetUserId();

            _goons.UpdateLastSeenOnUtc(userId);

            return Task.CompletedTask;
        }

        public async Task JoinGroup(string goonsquadId)
        {
            var userId = GetUserId();

            if (_goonsquads.GetGoonsquadIdsForGoon(userId).Contains(Guid.Parse(goonsquadId)))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, goonsquadId);
            }
        }

        public Task SendMessage(string content, string goonsquadId)
        {
            var userId = GetUserId();
            var message = _goonsquads.CreateMessage(content, Guid.Parse(goonsquadId), userId);

            if (_goonsquads.IsGoonMemberOfGoonsquad(userId, Guid.Parse(goonsquadId)))
            {
                return Clients.Group(goonsquadId).SendAsync("message", message);
            }
            return Task.CompletedTask;
        }
    }
}