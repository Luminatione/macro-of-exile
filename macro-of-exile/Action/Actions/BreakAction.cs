using MacroOfExile.Configuration;
using MacroOfExile.Macro.Context;
using Shared.Target;
using Shared.VirtualKeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Action.Actions
{
    //TODO: make this less brittle
    internal class BreakAction(string id, string onSuccess) : Action(id, null, onSuccess, string.Empty)
    {
        private const int KeyPressedState = 0x8000;

        public required VirtualKey Key { get; init; }

        private Thread? breakerThread;

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        public override void Execute(ITarget target, IContext context, MacroConfiguration? configuration)
        {
            breakerThread = new Thread(() => WaitForStopMessage(target));
            breakerThread.IsBackground = true;
            breakerThread.Start();
        }

        private void WaitForStopMessage(ITarget target)
        {
            if (target == null)
            {
                throw new InvalidOperationException("Cannot queue stop to unknown target");
            }

            while (!IsKeyPressed(Key))
            {
                Thread.Sleep(25);
            }

            target.QueueStop();
        }

        private bool IsKeyPressed(VirtualKey keyCode) 
        {
            return (GetAsyncKeyState((int) keyCode) & KeyPressedState) != 0;
        }
    }
}
