using MacroOfExile.Action.ActionResultResolver;
using MacroOfExile.Configuration;
using MacroOfExile.Macro.Context;
using Microsoft.CodeAnalysis.Scripting;
using Shared.Target;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MacroOfExile.Action.Actions
{
    [JsonDerivedType(typeof(RunPythonScriptAction), typeDiscriminator: "RunPythonScript")]
    public class RunPythonScriptAction(string id, IActionResultResolver? resolver, string onSuccess, string onFailure, bool isLast = false) : Action(id, resolver, onSuccess, onFailure, isLast)
    {
        public required string ScriptPath { get; set; }
        public string? Arguments { get; set; }

        public override void Execute(ITarget target, IContext context, MacroConfiguration? configuration)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = Arguments is null
                    ? $"\"{ScriptPath}\""
                    : $"\"{ScriptPath}\" {Arguments}",
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            using var process = Process.Start(psi);
            process?.WaitForExit();
        }
    }
}
