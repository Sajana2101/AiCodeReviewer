using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AiCodeReviewer.Models;
using System.Text.Json;

namespace AiCodeReviewer.Services
{
    public class ConfigService
    {
        private const string ConfigFileName = "aicr-config.json";

        public AppConfig Load()
        {
            if (!File.Exists(ConfigFileName))
            {
                return new AppConfig();
            }

            string json = File.ReadAllText(ConfigFileName);

            AppConfig? config =
                JsonSerializer.Deserialize<AppConfig>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

            return config ?? new AppConfig();
        }
    }
}