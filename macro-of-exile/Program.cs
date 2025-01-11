using MacroOfExile.Configuration;
using MacroOfExile.Macro;
using MacroOfExile.Macro.Context;
using MacroOfExile.Macro.MacroLoader;
using MacroOfExile.Target;

namespace MacroOfExile
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfigurationProvider configurationProvider = new JsonConfigurationProvider();
            IMacroLoader macroLoader = new JsonMacroLoader("macro.json", configurationProvider);
            var macro = macroLoader.CreateMacro();
            ITarget target = new ConsoleTarget();
            IMacroExecutor macroExecutor = new MacroExecutor(target, new MutableDictionaryContext());
            macroExecutor.Execute(macro);
        }
    }
}
