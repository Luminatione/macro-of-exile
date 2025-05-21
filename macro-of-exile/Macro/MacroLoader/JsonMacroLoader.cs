using MacroOfExile.Configuration;
using MacroOfExile.Macro.Context;
using MacroOfExile.Target;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MacroOfExile.Macro.MacroLoader
{
    public class JsonMacroLoader(string filepath, IConfigurationProvider configurationProvider, IFileSystem fileSystem) : IMacroLoader
    {
        public JsonMacroLoader(string filepath, IConfigurationProvider configurationProvider) : this (filepath, configurationProvider, new FileSystem()) { }

        private string Filepath { get; } = filepath;

        public Macro LoadMacro()
        {
            Macro macro = JsonDocument.Parse(fileSystem.File.ReadAllText(Filepath)).Deserialize<Macro>() ?? throw new JsonException($"Failed to parse file {Filepath}");
            macro.MacroConfiguration = configurationProvider.GetConfiguration();
            return macro;
        }
    }
}
