using System.Text.Json.Serialization;

namespace FactorioModUpdaterApp.Models
{
    public class Mod
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("enabled")]
        public bool enabled { get; set; }
    }

    public class ModList
    {
        [JsonPropertyName("mods")]
        public List<Mod> mods { get; set; }
    }
}
