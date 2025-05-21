using MacroOfExile.Configuration;
using MacroOfExile.Macro;
using MacroOfExile.Macro.Context;
using MacroOfExile.Macro.MacroLoader;
using MacroOfExile.Target;
using Shared.Target;

namespace MacroOfExile
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IConfigurationProvider configurationProvider = new JsonConfigurationProvider();
            IMacroLoader macroLoader = new JsonMacroLoader("macro.json", configurationProvider);
            var macro = macroLoader.LoadMacro();
            ITarget target = new VirtualDeviceInteractor.VirtualDeviceInteractor(@"\\.\VirtualDeviceDriver");
            IMacroExecutor macroExecutor = new MacroExecutor(target, new MutableDictionaryContext());
            macroExecutor.Execute(macro);
        }
    }
}
