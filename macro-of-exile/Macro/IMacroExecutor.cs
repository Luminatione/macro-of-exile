using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Macro
{
    public interface IMacroExecutor
    {
        void Before();
        void Execute(Macro macro);
        void After();
    }
}
