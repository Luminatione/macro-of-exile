using MacroOfExile.Configuration;
using MacroOfExile.Macro.Context;
using MacroOfExile.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Macro
{
    public class Macro(List<MacroOfExile.Action.Action> actions, IContext context, ITarget target, MacroConfiguration macroConfiguration)
    {
        List<Action.Action> Actions { get; } = actions ?? throw new ArgumentNullException(nameof(actions));
        IContext Context { get; } = context ?? throw new ArgumentNullException(nameof(context));
        ITarget Target { get; } = target ?? throw new ArgumentNullException(nameof(target));
        MacroConfiguration MacroConfiguration { get; } = macroConfiguration ?? new MacroConfiguration();
    }
}
