using MacroOfExile.Configuration;
using MacroOfExile.Exceptions;
using MacroOfExile.Macro.Context;
using Shared.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Action.Actions
{
    internal class InterpolateMouseToPositionAction(string id, string onSuccess, bool isLast = false) : Action(id, null, onSuccess, string.Empty, isLast)
    {
        public Evaluatebale Steps { get; init; } = new Evaluatebale("1");
        public required Evaluatebale X { get; set; }
        public required Evaluatebale Y { get; set; }

        [StructLayout(LayoutKind.Sequential)]
        protected struct Point
        {
            public int x;
            public int y;
        }

        [DllImport("user32.dll")]
        protected static extern bool GetCursorPos(out Point point);

        public override void Execute(ITarget target, IContext context, MacroConfiguration? configuration)
        {
            if (!GetCursorPos(out Point currentPosition))
            {
                throw new NotImplementedException();
            }

            if (!int.TryParse(Steps.GetValue(context), out int stepsValue))
            {
                throw new UncastableVariableException($"Failed to cast {nameof(Steps)}={Steps} to integer");
            }

            if (!int.TryParse(X.GetValue(context), out int targetPositionX) || !int.TryParse(Y.GetValue(context), out int targetPositionY))
            {
                throw new UncastableVariableException($"Failed to cast {nameof(X)}={X.GetValue(context)} {nameof(Y)}={Y.GetValue(context)} to integers");
            }

            int timePerStep = target.GetMilisBetweenActions() / stepsValue;
            int dx = (currentPosition.x - targetPositionX) / stepsValue;
            int dy = (currentPosition.y - targetPositionY) / stepsValue;

            for (int i = 0; i < stepsValue; i++)
            {
                //target.MoveMouse(currentPosition.x + dx, currentPosition.y + dy);
                Thread.Sleep(timePerStep);
            }
        }
    }
}
