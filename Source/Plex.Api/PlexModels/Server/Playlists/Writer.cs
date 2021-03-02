namespace Plex.Api.PlexModels.Server.Playlists
{
    using System.Text.Json.Serialization;

    public class Writer    
    {
        [JsonPropertyName("filter")]
        public string Filter { get; set; } 

        [JsonPropertyName("id")]
        public string Id { get; set; } 

        [JsonPropertyName("tag")]
        public string Tag { get; set; } 
    }
}