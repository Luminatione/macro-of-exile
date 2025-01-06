using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Exceptions
{
    public class UnknownVariableException : Exception
    {
        public UnknownVariableException() { }

        public UnknownVariableException(string message) : base(message) { }
    }
}
