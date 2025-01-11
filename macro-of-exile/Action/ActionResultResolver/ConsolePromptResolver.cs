using MacroOfExile.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Action.ActionResultResolver
{
    internal class ConsolePromptResolver : IActionResultResolver
    {
        public bool IsSuccess(ITarget target)
        {
            Console.WriteLine("Is success?: ");
            string response = Console.ReadLine() ?? "0";
            return response == "1";
        }
    }
}
