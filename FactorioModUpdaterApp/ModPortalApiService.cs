using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using FactorioModUpdaterApp.Models;

namespace FactorioModUpdaterApp
{
    public class ModPortalApiService
    {
        private const string _modInfoUrl = "https://mods.factorio.com/api/mods/{name}";
        private const string _modDownloadUrl = "https://mods.factorio.com/{download_url}?username={username}&token={token}";
        private readonly string _factorioVersion;
        private readonly string _username;
        private readonly string _token;
        private readonly HttpClient _client;

        public ModPortalApiService(string factorioVersion, string username, string token)
        {
            _factorioVersion = factorioVersion;
            _username = username;
            _token = token;
            _client = new HttpClient()
            {
                BaseAddress = new Uri("https://mods.factorio.com/", UriKind.Absolute)
            };
        }

        [RequiresDynamicCode("Calls System.Net.Http.Json.GetFromJsonAsync()")]
        [RequiresUnreferencedCode("Calls System.Net.Http.Json.GetFromJsonAsync()")]
        public Tuple<byte[], string>? DownloadMod(string modName, IEnumerable<string> existingFilenames)
        {
            var modApiModel = _client.GetFromJsonAsync<ModApiModel>($"/api/mods/{modName}").Result;
            if (modApiModel == null)
                return null;
            var latestRelease = modApiModel.releases.Where(w => w.info_json.factorio_version == _factorioVersion).OrderByDescending(o => SemanticVersion.Parse(o.version)).FirstOrDefault();
            if (latestRelease == null)
                return null;

            foreach (var existingFilename in existingFilenames)
            {
                if (latestRelease.file_name == Path.GetFileName(existingFilename))
                    return Tuple.Create(Array.Empty<byte>(), latestRelease.file_name);
            }
            var bytes = _client.GetByteArrayAsync($"{latestRelease.download_url}?username={_username}&token={_token}").Result;
            return Tuple.Create(bytes, latestRelease.file_name);
        }


    }
}
