using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace NeedsSoySauce.Entities
{
    public class Goonsquad
    {
        public Guid Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<Goon> Goons { get; set; }

        private Goonsquad()
        {
            CreatedOnUtc = DateTime.UtcNow;
        }

        public Goonsquad(ICollection<Goon> goons, string name) : this()
        {
            Goons = goons;
            Name = name;
        }

    }
}