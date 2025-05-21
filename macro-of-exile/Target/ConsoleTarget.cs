using Shared.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Target
{
    internal class ConsoleTarget : ITarget
    {
		public int GetMilisBetweenActions()
		{
            return 1;
		}

		public void MoveMouse(int x, int y)
        {
            Console.WriteLine($"Mouse moved to {x} {y}");
        }

        public void SetButtonState(int button, int state)
        {
            Console.WriteLine($"State of {button} set to {state}");
        }

        public void SetKeyState(int key, int state)
        {
            Console.WriteLine($"State of {key} set to {state}");
        }
    }
}
