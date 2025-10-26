using MacroOfExile.Macro.Context;
using Shared.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MacroOfExile.Action.ActionResultResolver
{
    internal class RegexOnClipboardResolver : IActionResultResolver
    {
        public required string Regex { get; set; }

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

        public bool IsSuccess(ITarget target, IContext context)
        {
            OpenClipboard(IntPtr.Zero);
            IntPtr handleToClipboard = GetClipboardData(1);
            string content = Marshal.PtrToStringAnsi(GlobalLock(handleToClipboard));
            GlobalUnlock(handleToClipboard);
            CloseClipboard();

            if (content == null)
            {
                return false;
            }

            Regex compiledRegex = new Regex(Regex);
            return compiledRegex.IsMatch(content);
        }
    }
}
