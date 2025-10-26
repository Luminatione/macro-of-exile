using MacroOfExile.Configuration;
using MacroOfExile.Macro.Context;
using MacroOfExile.Target;
using Shared.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Action.Actions
{
    public class InitializeContextAction(string id, string onSuccess) : Action(id, null, onSuccess, string.Empty)
    {
        public Dictionary<string, string> Variables { get; set; } = [];

        public override void Execute(ITarget target, IContext context, MacroConfiguration? configuration)
        {
            Variables.Keys.ToList().ForEach(k => context.SetVariable(k, Variables[k]));
        }
    }
}
