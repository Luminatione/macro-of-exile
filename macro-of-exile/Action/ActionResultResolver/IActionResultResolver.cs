using MacroOfExile.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MacroOfExile.Action.ActionResultResolver
{
    [JsonDerivedType(typeof(ConsolePromptResolver), typeDiscriminator: "ConsolePrompt")]
    public interface IActionResultResolver
    {
        bool IsSuccess(ITarget target);
    }
}
