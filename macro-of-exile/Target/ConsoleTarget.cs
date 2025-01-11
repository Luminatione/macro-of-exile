using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Target
{
    internal class ConsoleTarget : ITarget
    {
        public void MoveMouse(int x, int y)
        {
            Console.WriteLine($"Mouse moved to {x} {y}");
        }

        public void SetKeyState(int key, int state)
        {
            Console.WriteLine($"State of {key} set to {state}");
        }
    }
}
