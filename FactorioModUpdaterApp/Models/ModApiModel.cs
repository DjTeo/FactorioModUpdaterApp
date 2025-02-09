using System.Text.Json.Serialization;

namespace FactorioModUpdaterApp.Models
{
    public class Release
    {
        [JsonPropertyName("download_url")]
        public string download_url { get; set; }

        [JsonPropertyName("file_name")]
        public string file_name { get; set; }

        [JsonPropertyName("info_json")]
        public InfoJson info_json { get; set; }

        [JsonPropertyName("released_at")]
        public DateTime released_at { get; set; }

        [JsonPropertyName("sha1")]
        public string sha1 { get; set; }

        [JsonPropertyName("version")]
        public string version { get; set; }
    }

    public class ModApiModel
    {
        [JsonPropertyName("category")]
        public string category { get; set; }

        [JsonPropertyName("downloads_count")]
        public int downloads_count { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("owner")]
        public string owner { get; set; }

        [JsonPropertyName("releases")]
        public List<Release> releases { get; set; }

        [JsonPropertyName("score")]
        public double score { get; set; }

        [JsonPropertyName("summary")]
        public string summary { get; set; }

        [JsonPropertyName("thumbnail")]
        public string thumbnail { get; set; }

        [JsonPropertyName("title")]
        public string title { get; set; }
    }
}
