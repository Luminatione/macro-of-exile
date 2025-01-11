﻿using MacroOfExile.Action.ActionResultResolver;
using MacroOfExile.Exceptions;
using MacroOfExile.Macro.Context;
using MacroOfExile.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Action.Actions
{
    public class SingleClickAction(string id, IActionResultResolver? resolver, string onSuccess, string onFailure, bool isLast = false) : Action(id, resolver, onSuccess, onFailure, isLast)
    {
        public enum MouseButton
        {
            LMB = 0,
            RMB = 1,
            MMB = 3
        };

        public required Evaluatebale X { get; set; }
        public required Evaluatebale Y { get; set; }
        public required MouseButton Button { get; set; }

        public override void Execute(ITarget target, IContext context)
        {
            if (!int.TryParse(X.GetValue(context), out int xValue))
            {
                throw new UncastableVariableException($"Failed to cast variable to target type in expression {X.Expression}");
            }

            if (!int.TryParse(Y.GetValue(context), out int yValue))
            {
                throw new UncastableVariableException($"Failed to cast variable to target type in expression {Y.Expression}");
            }

            target.MoveMouse(xValue, yValue);
            target.SetKeyState((int) Button, 1);
            target.SetKeyState((int) Button, 0);
        }
    }
}
