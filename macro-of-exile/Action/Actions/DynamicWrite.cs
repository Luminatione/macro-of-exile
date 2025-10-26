using MacroOfExile.Action.ActionResultResolver;
using MacroOfExile.Configuration;
using MacroOfExile.Macro.Context;
using Shared.KeyState;
using Shared.Target;
using Shared.VirtualKeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Action.Actions
{
    internal class DynamicWrite(string id, IActionResultResolver? resolver, string onSuccess, string onFailure, bool isLast = false) : Action(id, resolver, onSuccess, onFailure, isLast)
    {
        [DllImport("user32.dll")]
        private static extern short VkKeyScan(char ch);

        public required Evaluatebale Text { get; set; }
        public override void Execute(ITarget target, IContext context, MacroConfiguration? configuration)
        {
            Text.GetValue(context).ToCharArray().ToList().ForEach(x => {
                target.SetKeysState([new KeyState((VirtualKey)(VkKeyScan(x) & 0xFF), 1)]);
                Thread.Sleep(target.GetMilisBetweenShortActions());
                target.SetKeysState([new KeyState((VirtualKey)(VkKeyScan(x) & 0xFF), 0)]);
                Thread.Sleep(target.GetMilisBetweenShortActions());
            });

            target.SetKeysState([new KeyState(0, 0)]);
        }
    }
}
