using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NeedsSoySauce.Models
{
    public class Game
    {

        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("background_image")]
        public string BackgroundImage { get; set; }

        public Game(string name, string backgroundImage)
        {
            Name = name;
            BackgroundImage = backgroundImage;
        }
    }
}