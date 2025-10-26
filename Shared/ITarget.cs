using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Target
{
    public interface ITarget
    {
        void MoveMouse(int x, int y);
        void SetKeysState(List<KeyState.KeyState> keys);
        void SetMouseState(int button, int state, short x, short y);
        int GetMilisBetweenActions();
        int GetMilisBetweenShortActions();
        void QueueStop();
        bool IsStopQueued();
    }
}
