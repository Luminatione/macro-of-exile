using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MacroOfExile.Configuration
{
    public class JsonConfigurationProvider(IFileSystem fileSystem) : IConfigurationProvider
    {
        private readonly IFileSystem fileSystem = fileSystem;

        public JsonConfigurationProvider() : this(fileSystem: new FileSystem()) { }

        public string ConfigurationFilename { get; set; } = "configuration.json";

        public MacroConfiguration GetConfiguration()
        {
            return JsonSerializer.Deserialize<MacroConfiguration>(JsonDocument.Parse(fileSystem.File.ReadAllText(ConfigurationFilename))) ?? new MacroConfiguration();
        }
    }
}
