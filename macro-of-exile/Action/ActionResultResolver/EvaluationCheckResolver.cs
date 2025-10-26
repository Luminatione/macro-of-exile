using MacroOfExile.Exceptions;
using MacroOfExile.Macro.Context;
using Shared.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Action.ActionResultResolver
{
    internal class EvaluationCheckResolver : IActionResultResolver
    {
        public required Evaluatebale Value { get; init; }

        public bool IsSuccess(ITarget target, IContext context)
        {
            if (!bool.TryParse(Value.GetValue(context), out var value))
            {
                throw new UncastableVariableException();
            }

            return value;
        }
    }
}
