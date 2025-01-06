using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Macro.Context
{
    public interface IContext
    {
        T GetVariable<T>(string name);
        void SetVariable<T>(string name, T variableValue);
        void ModifyVariable<T>(string name, Action<T> action);
    }
}
