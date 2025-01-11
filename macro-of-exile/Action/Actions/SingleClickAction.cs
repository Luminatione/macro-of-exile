using MacroOfExile.Action.ActionResultResolver;
using MacroOfExile.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Action.Actions
{
    public class SingleClickAction(string id, IActionResultResolver resolver, string onSuccess, string onFailure, bool isLast = false) : Action(id, resolver, onSuccess, onFailure, isLast)
    {
        public enum MouseButton
        {
            LMB = 0,
            RMB = 1,
            MMB = 3
        };

        public int X { get; set; }
        public int Y { get; set; }
        public MouseButton Button { get; set; }

        public override void Execute(ITarget target)
        {
            target.MoveMouse(X, Y);
            target.SetKeyState((int) Button, 1);
            target.SetKeyState((int) Button, 0);
        }
    }
}
