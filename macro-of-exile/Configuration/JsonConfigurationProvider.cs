using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MacroOfExile.Configuration
{
    public class JsonConfigurationProvider : IConfigurationProvider
    {
        public string ConfigurationFilename { get; set; } = "configuration.json";

        public MacroConfiguration GetConfiguration()
        {
            return JsonSerializer.Deserialize<MacroConfiguration>(JsonDocument.Parse(File.ReadAllText(ConfigurationFilename))) ?? new MacroConfiguration();
        }
    }
}
