using MacroOfExile.Action.ActionResultResolver;
using MacroOfExile.Configuration;
using MacroOfExile.Macro.Context;
using Shared.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Action.Actions
{
    internal class ClipboardToVariableAction(string id, IActionResultResolver resolver, string onSuccess, string onFailure, bool isLast = false) : Action(id, resolver, onSuccess, onFailure, isLast)
    {
        public required string Variable { get; set; }

        [DllImport("user32.dll")]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        private static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        private static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern bool GlobalUnlock(IntPtr hMem);

        public override void Execute(ITarget target, IContext context, MacroConfiguration configuration)
        {
            try
            {
                OpenClipboard(IntPtr.Zero);
                IntPtr handle = GetClipboardData(1);
                string content = null;
                if (handle != IntPtr.Zero)
                {
                    IntPtr pointer = GlobalLock(handle);
                    if (pointer != IntPtr.Zero)
                    {
                        content = Marshal.PtrToStringAnsi(pointer);
                        GlobalUnlock(handle);
                    }
                }
                CloseClipboard();

                if (content != null)
                {
                    context.SetVariable(Variable, content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClipboardToVariableAction error: {ex}");
            }
        }
    }
}
