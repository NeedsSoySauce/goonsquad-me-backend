using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace NeedsSoySauce.Entities
{
    public class Job
    {
        public Guid Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string GoonId { get; set; }
        [JsonIgnore]
        public Goon Goon { get; set; }

        private Job()
        {
            CreatedOnUtc = DateTime.UtcNow;
        }

        public Job(Goon goon) : this()
        {
            Goon = goon;
        }

    }
}