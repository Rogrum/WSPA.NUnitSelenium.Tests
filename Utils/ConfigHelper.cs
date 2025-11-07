using Microsoft.Extensions.Configuration;
using System;
namespace WPSF.NUnitSelenium.Tests.Utils
{
    public static class ConfigHelper
    {
        private static readonly IConfigurationRoot _config =
            new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .Build();

        public static string GetString(string key, string fallback="") => _config[key] ?? fallback;
        public static bool GetBool(string key, bool fallback=false) => bool.TryParse(_config[key], out var b) ? b : fallback;
        public static int GetInt(string key, int fallback=0) => int.TryParse(_config[key], out var i) ? i : fallback;
    }
}
