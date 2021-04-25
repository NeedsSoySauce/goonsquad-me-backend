using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NeedsSoySauce.Entities
{
    public class Goon
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public DateTime LastSeenOnUtc { get; set; }

        [JsonIgnore]
        public ICollection<Goonsquad> Goonsquads { get; set; }

        private Goon()
        {
            // Used by EF Core
        }

        public Goon(string id, string username, ICollection<Goonsquad> goonsquads) : this()
        {
            Id = id;
            Username = username;
            Goonsquads = goonsquads;
        }
    }
}