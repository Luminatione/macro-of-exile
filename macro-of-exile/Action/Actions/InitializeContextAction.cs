using MacroOfExile.Macro.Context;
using MacroOfExile.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Action.Actions
{
    public class InitializeContextAction(string id, string next) : Action(id, null, next, string.Empty)
    {
        public Dictionary<string, string> Variables { get; set; } = [];

        public override void Execute(ITarget target, IContext context)
        {
            Variables.Keys.ToList().ForEach(k => context.SetVariable(k, Variables[k]));
        }
    }
}
