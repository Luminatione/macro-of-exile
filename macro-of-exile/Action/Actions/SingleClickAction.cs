using MacroOfExile.Action.ActionResultResolver;
using MacroOfExile.Action.Actions.Enums;
using MacroOfExile.Exceptions;
using MacroOfExile.Macro.Context;
using MacroOfExile.Target;
using MacroOfExile.Configuration;
using Shared.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Action.Actions
{
    public class SingleClickAction(string id, IActionResultResolver? resolver, string onSuccess, string onFailure, bool isLast = false) : Action(id, resolver, onSuccess, onFailure, isLast)
    {
        public required Evaluatebale X { get; set; }
        public required Evaluatebale Y { get; set; }
        public required MouseButton Button { get; set; }

        public override void Execute(ITarget target, IContext context, MacroConfiguration? configuration)
        {
            if (!int.TryParse(X.GetValue(context), out int xValue))
            {
                throw new UncastableVariableException($"Failed to cast variable to target type in expression {X.Expression}");
            }

            if (!int.TryParse(Y.GetValue(context), out int yValue))
            {
                throw new UncastableVariableException($"Failed to cast variable to target type in expression {Y.Expression}");
            }

            target.SetMouseState(0, 0, ToScreenSpace(xValue, configuration?.ResolutionX ?? short.MaxValue), ToScreenSpace(yValue, configuration?.ResolutionY ?? short.MaxValue));
            Thread.Sleep(target.GetMilisBetweenShortActions());
			target.SetMouseState((int) Button, 1, ToScreenSpace(xValue, configuration?.ResolutionX ?? short.MaxValue), ToScreenSpace(yValue, configuration?.ResolutionY ?? short.MaxValue));
            Thread.Sleep(target.GetMilisBetweenShortActions());
			target.SetMouseState((int) Button, 0, ToScreenSpace(xValue, configuration?.ResolutionX ?? short.MaxValue), ToScreenSpace(yValue, configuration?.ResolutionY ?? short.MaxValue));
		}
        private short ToScreenSpace(int value, int resolution)
        {
            return (short) ((float) value / resolution * short.MaxValue);
        }

    }
}
