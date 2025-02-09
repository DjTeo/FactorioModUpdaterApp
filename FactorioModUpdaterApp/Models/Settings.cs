namespace FactorioModUpdaterApp.Models
{
    public sealed class Settings
    {
        public required string ModListFile { get; set; }
        public required string ModFolder { get; set; }
        public required string Username { get; set; }
        public required string Token { get; set; }
        public required string FactorioVersion { get; set; }
        public List<string> ModsToIgnonre { get; set; }
    }
}
