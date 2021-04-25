using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using NeedsSoySauce.Data;
using NeedsSoySauce.Models;
using RestSharp;

namespace NeedsSoySauce.Services.RawgApiGamesService
{

    /// <summary>
    /// Minimal wrapper around https://api.rawg.io/docs/#tag/games.
    /// </summary>
    public class RawgApiGamesService : IGamesService
    {
        private class RawgApiGamesResponse
        {
            [JsonPropertyName("count")]
            public int Count { get; set; }
            [JsonPropertyName("next")]
            public string? Next { get; set; }
            [JsonPropertyName("previous")]
            public string? Previous { get; set; }
            [JsonPropertyName("results")]
            public IList<Game> Results { get; set; }

            public RawgApiGamesResponse(IList<Game> results)
            {
                Results = results;
            }
        }

        private const string API_ENDPOINT = "https://api.rawg.io/api";
        private string _apiKey;

        public RawgApiGamesService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public PagedResult<Game> GetGames(int page, int pageSize)
        {
            var client = new RestClient(API_ENDPOINT);
            client.AddDefaultQueryParameter("key", _apiKey);

            var request = new RestRequest("games", Method.GET, DataFormat.Json);
            request.AddQueryParameter("page", page.ToString());
            request.AddQueryParameter("page_size", pageSize.ToString());

            var content = client.Execute(request).Content;
            var response = JsonSerializer.Deserialize<RawgApiGamesResponse>(content) ?? throw new ArgumentNullException();

            return new PagedResult<Game>(response.Results)
            {
                HasNext = !string.IsNullOrEmpty(response.Next),
                HasPrevious = !string.IsNullOrEmpty(response.Previous),
                Page = page,
                PageSize = pageSize,
                Pages = (int)Math.Ceiling((double)response.Count / pageSize)
            };
        }
    }
}