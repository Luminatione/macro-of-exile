using MacroOfExile.Configuration;
using MacroOfExile.Macro;
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
            Macro.Macro macro = macroLoader.CreateMacro();
            ITarget target = new ConsoleTarget();
            IMacroExecutor macroExecutor = new MacroExecutor(target);
            macroExecutor.Execute(macro);
        }
    }
}
