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
    public class Macro(List<Action.Action> actions, MacroConfiguration macroConfiguration)
    {
        public List<Action.Action> Actions { get; set; } = actions ?? throw new ArgumentNullException(nameof(actions));
        public MacroConfiguration MacroConfiguration { get; set; } = macroConfiguration ?? new MacroConfiguration();
    }
}
