using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDeviceInteractor
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct InputMessage
    {
        public readonly char xAxis;
        public readonly char yAxis;
        public readonly int buttons;
        public readonly int key;
        public readonly int keyState;
        public readonly byte modifiers;

        public InputMessage(char xAxis = '\0', char yAxis = '\0', int buttons = 0, int key = 0, int keyState = 0, byte modifiers = 0)
        {
            this.xAxis = xAxis;
            this.yAxis = yAxis;
            this.buttons = buttons;
            this.key = key;
            this.keyState = keyState;
            this.modifiers = modifiers;
        }
    }
}
