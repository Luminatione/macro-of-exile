using MacroOfExile.Exceptions;
using MacroOfExile.Macro.Context;
using MacroOfExile.Target;
using Shared.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Macro
{
    public class MacroExecutor(ITarget target, IContext context) : IMacroExecutor
    {
        public void After()
        {
            
        }

        public void Before()
        {
            
        }

        public void Execute(Macro macro)
        {
            Action.Action action = macro.Actions.Where(a => a.Id == "0").FirstOrDefault() ?? throw new MissingFirstMacroElementException("Macro has to contain element with ID equal 0");
            while (!target.IsStopQueued())
            {
                action.Execute(target, context, macro.MacroConfiguration);
                if (action.IsLast)
                {
                    break;
                }

                string nextId = action.GetNext(target, context);
                action = macro.Actions.Where(a => a.Id == nextId).First();
                Thread.Sleep(target.GetMilisBetweenActions());
            } 
        }
    }
}
