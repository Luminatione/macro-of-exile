using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.KeyState
{
    public class KeyState
    {
        public VirtualKeys.VirtualKey Key { get; }
        public int State { get; }

        public KeyState(VirtualKeys.VirtualKey key, int state) 
        {
            this.Key = key;
            this.State = state;
        }
    }
}
