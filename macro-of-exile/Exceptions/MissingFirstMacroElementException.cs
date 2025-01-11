using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Exceptions
{
    public class MissingFirstMacroElementException : Exception
    {
        public MissingFirstMacroElementException() { }

        public MissingFirstMacroElementException(string message) : base(message) { }
    }
}
