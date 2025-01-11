using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Exceptions
{
    public class UncastableVariableException : Exception
    {
        public UncastableVariableException() { }

        public UncastableVariableException(string message) : base(message) { }
    }
}
