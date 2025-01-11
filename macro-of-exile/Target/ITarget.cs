using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroOfExile.Target
{
    public interface ITarget
    {
        void MoveMouse(int x, int y);
        void SetKeyState(int key, int state);
    }
}
