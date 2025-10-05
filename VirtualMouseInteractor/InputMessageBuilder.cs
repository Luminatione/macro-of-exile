using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDeviceInteractor
{
    internal class InputMessageBuilder
    {
        private VKToDirverKeyCodeTranslator VKToDirverKeyCodeTranslator = new VKToDirverKeyCodeTranslator();

        private char xAxis = '\0';
        private char yAxis = '\0';
        private List<(int key, int state)> keys = new List<(int, int)>();
        private int buttons = 0;
        private int modifiers = 0;

        public InputMessage Build()
        {
            return new InputMessage(xAxis, yAxis, buttons, keys.FirstOrDefault().key, keys.FirstOrDefault().state, modifiers);
        }

        public InputMessageBuilder Move(char xAxis, char yAxis)
        {
            this.xAxis = xAxis;
            this.yAxis = yAxis;
            return this;
        }

        public InputMessageBuilder Key(int keyCode, int state)
        {
            keys.Add((VKToDirverKeyCodeTranslator.GetDriverKey(keyCode), state));
            return this;
        }

        public InputMessageBuilder Button(int buttonCode, int state)
        {
            buttons |= state << VKToDirverKeyCodeTranslator.GetDriverKey(buttonCode);
            return this;
        }

        public InputMessageBuilder Modifier(int modifierCode)
        {
            modifiers |= modifierCode;
            return this;
        }

        public InputMessageBuilder WithVKCode(int code, int status)
        {
            if (VKToDirverKeyCodeTranslator.IsKeyboardCharacter(code))
            {
                return Key(code, status);
            }

            if (VKToDirverKeyCodeTranslator.IsMouseButton(code))
            {
                return Button(code, status);
            }

            if (VKToDirverKeyCodeTranslator.IsModifierKey(code))
            {
                return Modifier(code);
            }

            throw new ArgumentException($"Code {code} is not valid button code");
        }
    }
}
