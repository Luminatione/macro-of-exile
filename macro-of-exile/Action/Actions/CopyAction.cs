using MacroOfExile.Action.ActionResultResolver;
using MacroOfExile.Configuration;
using MacroOfExile.Macro.Context;
using Shared.KeyState;
using Shared.Target;
using Shared.VirtualKeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Action.Actions
{
    internal class CopyAction(string id, IActionResultResolver resolver, string onSuccess, string onFailure, bool isLast = false) : Action(id, resolver, onSuccess, onFailure, isLast)
    {
        public override void Execute(ITarget target, IContext context, MacroConfiguration configuration)
        {
            Thread.Sleep(target.GetMilisBetweenActions());
            target.SetKeysState([new KeyState(VirtualKey.C, 1), new KeyState(VirtualKey.LControl, 1), new KeyState(VirtualKey.LAlt, 1)]);
            Thread.Sleep(target.GetMilisBetweenActions());
            target.SetKeysState([new KeyState(VirtualKey.C, 0), new KeyState(VirtualKey.LControl, 0), new KeyState(VirtualKey.LAlt, 0)]);
            Thread.Sleep(target.GetMilisBetweenActions());
            target.SetKeysState([new KeyState(0, 0)]);
        }
    }
}
