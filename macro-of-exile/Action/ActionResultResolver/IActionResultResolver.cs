using MacroOfExile.Macro.Context;
using MacroOfExile.Target;
using Shared.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MacroOfExile.Action.ActionResultResolver
{
    [JsonDerivedType(typeof(ConsolePromptResolver), typeDiscriminator: "ConsolePrompt")]
    [JsonDerivedType(typeof(RegexOnClipboardResolver), typeDiscriminator: "RegexOnClipboard")]
    [JsonDerivedType(typeof(EvaluationCheckResolver), typeDiscriminator: "EvaluationCheck")]

    public interface IActionResultResolver
    {
        bool IsSuccess(ITarget target, IContext context);
    }
}
