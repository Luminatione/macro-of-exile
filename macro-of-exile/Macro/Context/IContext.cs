using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Macro.Context
{
    public interface IContext
    {
        string GetVariable(string name);
        void SetVariable(string name, string variableValue);
        void ModifyVariable(string name, Action<string> action);
    }
}
