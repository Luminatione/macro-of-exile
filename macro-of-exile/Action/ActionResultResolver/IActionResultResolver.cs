using MacroOfExile.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Action.ActionResultResolver
{
    public interface IActionResultResolver
    {
        string GetResult(ITarget target);
    }
}
