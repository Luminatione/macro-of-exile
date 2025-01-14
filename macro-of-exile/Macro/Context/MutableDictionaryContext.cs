using MacroOfExile.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Macro.Context
{
    public class MutableDictionaryContext : IContext
    {
        private readonly Dictionary<string, string> values = [];

        public string GetVariable(string name)
        {
            if (values.TryGetValue(name, out var value))
            {
                return value;
            }

            throw new UnknownVariableException($"Variable {name} is not known");
        }

        public void ModifyVariable(string name, Func<string, string> action)
        {
            if (values.TryGetValue(name, out var value))
            {
                values[name] = action(value);
                return;
            }

            throw new UnknownVariableException($"Variable {name} is not known");
        }

        public void SetVariable(string name, string variableValue)
        {
            values[name] = variableValue;
        }
    }
}
