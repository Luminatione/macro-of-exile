using MacroOfExile.Configuration;
using MacroOfExile.Macro.Context;
using MacroOfExile.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MacroOfExile.Macro.MacroLoader
{
    internal class JsonMacroLoader(string filepath, IConfigurationProvider configurationProvider) : IMacroLoader
    {
        private string Filepath { get; } = filepath;

        public Macro CreateMacro()
        {
            Macro macro = JsonDocument.Parse(File.ReadAllText(Filepath)).Deserialize<Macro>() ?? throw new JsonException($"Failed to parse file {Filepath}");
            macro.MacroConfiguration = configurationProvider.GetConfiguration();
            return macro;
        }
    }
}
