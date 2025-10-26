using Shared.KeyState;
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
        private volatile bool isStopQueued = false;

		public int GetMilisBetweenActions()
		{
            return 1;
		}

		public void MoveMouse(int x, int y)
        {
            Console.WriteLine($"Mouse moved to {x} {y}");
        }

        public void SetMouseState(int button, int state, short x, short y)
        {
            Console.WriteLine($"State of {button} set to {state}. Mouse moved to {x} {y}");
        }

        public void SetKeysState(List<KeyState> keyStates)
        {
            keyStates.ForEach(x => Console.WriteLine($"State of {x.Key} set to {x.State}"));
        }

        public void QueueStop()
        {
            Console.WriteLine("Stop Queued");
            isStopQueued = true;
        }

        public bool IsStopQueued() => isStopQueued;
    }
}
