using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MacroOfExile.Action.ActionResultResolver;
using MacroOfExile.Exceptions;
using MacroOfExile.Macro.Context;
using Shared.Target;

namespace MacroOfExile.Action.Actions
{
    public partial class SetKeyStateAction(string id, IActionResultResolver? resolver, string onSuccess, string onFailure, bool isLast = false) : Action(id, resolver, onSuccess, onFailure, isLast)
    {
        public required VirtualKey Key { get; set; }
        public required int State { get; set; }

        public override void Execute(ITarget target, IContext context)
        {
            target.SetKeyState((int) Key, State);
        }
    }
}
