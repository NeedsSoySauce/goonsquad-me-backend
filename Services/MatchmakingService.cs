using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NeedsSoySauce.Data;
using NeedsSoySauce.Entities;
using NeedsSoySauce.Models;
using NeedsSoySauce.Repositories;
using NeedsSoySauce.SignalR;

namespace NeedsSoySauce.Services
{
    // Based on: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-5.0&tabs=visual-studio#timed-background-tasks
    public class MatchmakingService : IHostedService, IDisposable
    {
        private readonly ILogger<MatchmakingService> _logger;
        private Timer? _timer;
        private IServiceProvider _services;

        public MatchmakingService(ILogger<MatchmakingService> logger, IServiceProvider services)
        {
            _services = services;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            _logger.LogInformation("Executing matchmaking service...");
            Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();

            using (var scope = _services.CreateScope())
            {
                var scopedService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();
                scopedService.DoWork().Wait();
            }

            stopWatch.Stop();
            _logger.LogInformation($"Executed matchmaking service in {stopWatch.ElapsedMilliseconds} milliseconds");
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        internal interface IScopedProcessingService
        {
            Task DoWork();
        }

        internal class ScopedMatchmakingService : IScopedProcessingService
        {
            private ILogger _logger;
            private ApplicationDbContext _context;
            private IJobsRepo _jobs;
            private IGoonsquadsRepo _goonsquads;
            private IHubContext<ChatHub> _hubContext;

            public ScopedMatchmakingService(ILogger<ScopedMatchmakingService> logger,
                                            ApplicationDbContext context,
                                            IJobsRepo jobs,
                                            IGoonsquadsRepo goonsquads,
                                            IHubContext<ChatHub> hubContext)
            {
                _logger = logger;
                _context = context;
                _jobs = jobs;
                _goonsquads = goonsquads;
                _hubContext = hubContext;
            }

            public async Task DoWork()
            {
                // The 'matching' in this case is entirely based on whether people are online around the same time
                var query = from Job in _context.Set<Job>()
                            join Goon in _context.Set<Goon>().Where(g => g.LastSeenOnUtc > DateTime.UtcNow.AddSeconds(-15))
                            on Job.GoonId equals Goon.Id
                            orderby Job.CreatedOnUtc
                            select new { Job, Goon };

                // Console.WriteLine(query.ToQueryString());

                int batchSize = 20;
                int groupSize = 2;

                var items = query.Take(batchSize).ToList();

                if (items.Count < groupSize) return;

                var jobs = new List<Job>();
                var groupMembers = new List<Goon>();
                var tasks = new List<Task>();

                for (int i = 0; i < items.Count; i++)
                {
                    jobs.Add(items[i].Job);
                    groupMembers.Add(items[i].Goon);

                    if (groupMembers.Count == groupSize)
                    {
                        // TODO: Generate random name
                        var goonsquad = _goonsquads.CreateGoonsquad("123", groupMembers);

                        foreach (var job in jobs)
                        {
                            _jobs.RemoveJob(job.Id);
                        }

                        // Notify clients they've been matched
                        var notification = new GoonsquadMembershipNotification()
                        {
                            GoonsquadId = goonsquad.Id
                        };
                        tasks.Add(_hubContext.Clients.Users(groupMembers.Select(s => s.Id)).SendAsync("notify", notification));

                        jobs = new List<Job>();
                        groupMembers = new List<Goon>();
                    }
                }

                await Task.WhenAll(tasks);
            }
        }
    }
}
