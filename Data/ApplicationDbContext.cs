using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NeedsSoySauce.Entities;

namespace NeedsSoySauce.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Goon> Goons => Set<Goon>();
        public DbSet<Goonsquad> Goonsquads => Set<Goonsquad>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Job> Jobs => Set<Job>();


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}