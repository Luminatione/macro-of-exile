using MacroOfExile.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Macro
{
    public class MacroExecutor(ITarget target) : IMacroExecutor
    {

        public void After()
        {
            
        }

        public void Before()
        {
            
        }

        public void Execute(Macro macro)
        {
            Action.Action action = macro.Actions.Where(a => a.Id == "0").First();
            while (!action.IsLast)
            {
                action.Execute(target);
                string nextId = action.GetNext(target);
                action = macro.Actions.Where(a => a.Id == nextId).First();
            }
        }
    }
}
