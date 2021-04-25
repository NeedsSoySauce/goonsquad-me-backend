using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NeedsSoySauce.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public DateTime SentOnUtc { get; set; }
        public string Content { get; set; }
        public string GoonId { get; set; }
        public string GoonUsername { get; set; }
        [JsonIgnore]
        public Goon Goon { get; set; }
        public Guid GoonsquadId { get; set; }
        [JsonIgnore]
        public Goonsquad Goonsquad { get; set; }

        private Message()
        {
            SentOnUtc = DateTime.UtcNow;
        }

        public Message(string content, Goonsquad goonsquad, Goon goon, string goonUsername) : this()
        {
            Content = content;
            Goonsquad = goonsquad;
            Goon = goon;
            GoonUsername = goonUsername;
        }
    }
}