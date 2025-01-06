using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Macro.Context
{
    public interface IContext
    {
        T GetVariable<T>(string name) where T : class;
        void SetVariable<T>(string name, T variableValue) where T : class;
        void ModifyVariable<T>(string name, Action<T> action) where T : class;
    }
}
