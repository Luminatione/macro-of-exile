using MacroOfExile.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Macro.Context
{
    internal class MutableDictionaryContext : IContext
    {
        protected class Variable
        {
            public required Type type;
            public required object value;
        }

        private Dictionary<string, Variable> values = new Dictionary<string, Variable>();

        public T GetVariable<T>(string name) where T : class
        {
            if (values.TryGetValue(name, out var value))
            {
                return value.value as T ?? throw new InvalidCastException($"{name} cannot be get as {typeof(T).Name} since it's {value.type.Name}");
            }

            throw new UnknownVariableException($"Variable {name} is not known");
        }

        public void ModifyVariable<T>(string name, Action<T> action) where T : class
        {
            if (values.TryGetValue(name, out var value))
            {
                action((T) value.value);
                return;
            }

            throw new UnknownVariableException($"Variable {name} is not known");
        }

        public void SetVariable<T>(string name, T variableValue) where T : class
        {
            values[name].value = variableValue;
        }
    }
}
