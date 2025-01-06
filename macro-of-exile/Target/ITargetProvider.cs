using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Target
{
    public interface ITargetProvider
    {
        ITarget GetTarget();
    }
}
