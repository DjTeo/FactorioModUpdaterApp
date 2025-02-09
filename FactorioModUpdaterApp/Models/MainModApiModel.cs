using System.Text.Json.Serialization;

namespace FactorioModUpdaterApp.Models
{
    public class InfoJson
    {
        [JsonPropertyName("factorio_version")]
        public string factorio_version { get; set; }
    }

    public class LatestRelease
    {
        [JsonPropertyName("download_url")]
        public string download_url { get; set; }

        [JsonPropertyName("file_name")]
        public string file_name { get; set; }

        [JsonPropertyName("info_json")]
        public InfoJson info_json { get; set; }

        [JsonPropertyName("released_at")]
        public DateTime released_at { get; set; }

        [JsonPropertyName("version")]
        public string version { get; set; }

        [JsonPropertyName("sha1")]
        public string sha1 { get; set; }
    }

    public class Links
    {
        [JsonPropertyName("first")]
        public object first { get; set; }

        [JsonPropertyName("next")]
        public string next { get; set; }

        [JsonPropertyName("prev")]
        public object prev { get; set; }

        [JsonPropertyName("last")]
        public string last { get; set; }
    }

    public class Pagination
    {
        [JsonPropertyName("count")]
        public int count { get; set; }

        [JsonPropertyName("page")]
        public int page { get; set; }

        [JsonPropertyName("page_count")]
        public int page_count { get; set; }

        [JsonPropertyName("page_size")]
        public int page_size { get; set; }

        [JsonPropertyName("links")]
        public Links links { get; set; }
    }

    public class Result
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("title")]
        public string title { get; set; }

        [JsonPropertyName("owner")]
        public string owner { get; set; }

        [JsonPropertyName("summary")]
        public string summary { get; set; }

        [JsonPropertyName("downloads_count")]
        public int downloads_count { get; set; }

        [JsonPropertyName("category")]
        public string category { get; set; }

        [JsonPropertyName("score")]
        public double score { get; set; }

        [JsonPropertyName("latest_release")]
        public LatestRelease latest_release { get; set; }
    }

    public class MainModApiModel
    {
        [JsonPropertyName("pagination")]
        public Pagination pagination { get; set; }

        [JsonPropertyName("results")]
        public List<Result> results { get; set; }
    }
}
