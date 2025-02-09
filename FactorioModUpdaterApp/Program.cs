using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using FactorioModUpdaterApp.Models;
using Microsoft.Extensions.Configuration;

namespace FactorioModUpdaterApp
{
    internal class Program
    {

        [RequiresDynamicCode("Calls Microsoft.Extensions.Configuration.ConfigurationBinder.Get<T>()")]
        [RequiresUnreferencedCode("Calls Microsoft.Extensions.Configuration.ConfigurationBinder.Get<T>()")]
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Factorio Mod Updater!");

            IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();

            var settings = config.GetSection("Settings").Get<Settings>();
            if (settings == null)
            {
                Exit("Missing 'Settings' section on appsettings.json");
                return;
            }
            string filePath = GetFullPath(settings.ModListFile);

            Console.WriteLine("Loading mod list: " + filePath);
            if (!File.Exists(filePath))
            {
                Exit("File not found: " + filePath);
                return;
            }
            var modFolder = GetFullPath(settings.ModFolder);
            if (!Path.EndsInDirectorySeparator(modFolder))
                modFolder += Path.DirectorySeparatorChar;

            if (!Directory.Exists(modFolder))
            {
                Console.WriteLine("ModFolder not found, creating folder: " + modFolder);
                Directory.CreateDirectory(modFolder);
            }
            Console.WriteLine("Mods will be downloaded to folder: " + modFolder);

            var jsonString = File.ReadAllText(filePath);
            var modList = JsonSerializer.Deserialize<ModList>(jsonString);
            if (modList == null || modList.mods == null)
            {
                Exit("Mod List is empty");
                return;
            }
            var enabledMods = modList.mods!.Where(w => w.enabled).ToList();

            var username = settings.Username;
            if (string.IsNullOrEmpty(username))
            {
                Exit("Username is empty");
                return;
            }
            var token = settings.Token;
            if (string.IsNullOrEmpty(token))
            {
                Exit("Token is empty");
                return;
            }
            var factorioVersion = settings.FactorioVersion;
            var modPortalApiService = new ModPortalApiService(factorioVersion, username, token);

            var modsToIgnonre = settings.ModsToIgnore;
            if (modsToIgnonre == null)
                modsToIgnonre = new List<string>() { "base" };


            foreach (var mod in enabledMods)
            {
                if (modsToIgnonre.Contains(mod.name))
                    continue;

                var existingFilenames = Directory.EnumerateFiles(modFolder, Path.GetFileNameWithoutExtension(mod.name) + "*", SearchOption.TopDirectoryOnly);
                var bytes_filename = modPortalApiService.DownloadMod(mod.name, existingFilenames);

                if (bytes_filename == null)
                {
                    Console.WriteLine($"Mod Releases for '{mod}' not found");
                    continue;
                }

                if (bytes_filename.Item1.Length != 0)
                {
                    foreach (var existingFilename in existingFilenames)
                    {
                        Console.WriteLine($"Deleting file '{Path.GetFileName(existingFilename)}'");
                        File.Delete(existingFilename);
                    }

                    var modFileName = bytes_filename.Item2;
                    Console.WriteLine($"Downloading file '{modFileName}'");
                    File.WriteAllBytes(Path.Combine(modFolder, modFileName), bytes_filename.Item1);
                }
                else
                {
                    foreach (var existingFilename in existingFilenames)
                    {
                        if (bytes_filename.Item2 == Path.GetFileName(existingFilename))
                            continue;
                        Console.WriteLine($"Deleting file '{Path.GetFileName(existingFilename)}'");
                        File.Delete(existingFilename);
                    }
                    Console.WriteLine($"Mod already exist '{bytes_filename.Item2}'");
                }
            }

            Console.WriteLine($"Mods updated successfully");
            Console.WriteLine($"Exiting...");
            Thread.Sleep(3000);
        }

        private static string GetFullPath(string path)
        {
            if (!Path.IsPathFullyQualified(path))
                return Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, path));
            return path;
        }

        private static void Exit(string error)
        {
            Console.WriteLine("ERROR!");
            Console.WriteLine(error);
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
