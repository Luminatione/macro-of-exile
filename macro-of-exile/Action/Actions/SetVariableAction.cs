using MacroOfExile.Action.ActionResultResolver;
using MacroOfExile.Configuration;
using MacroOfExile.Macro.Context;
using Shared.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Action.Actions
{
    internal class SetVariableAction(string id, IActionResultResolver? resolver, string onSuccess, string onFailure, bool isLast = false) : Action(id, resolver, onSuccess, onFailure, isLast)
    {
        public required string Variable { get; init; }
        public required Evaluatebale Value { get; init; }

        public override void Execute(ITarget target, IContext context, MacroConfiguration? configuration)
        {
            context.SetVariable(Variable, Value.GetValue(context));
        }
    }
}
