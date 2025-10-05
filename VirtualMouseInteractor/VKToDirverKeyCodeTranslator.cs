using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDeviceInteractor
{
	public class VKToDirverKeyCodeTranslator
	{
		private static readonly Dictionary<int, int> VkToHid = new Dictionary<int, int>()
		{
			//terminator
            [0x0] = 0,

			//Mouse
            [0x01] = 0x01, // VK_LBUTTON
            [0x02] = 0x02, // VK_RBUTTON
            [0x04] = 0x03, // VK_MBUTTON

            // Letters
            [0x41] = 0x04, // A
			[0x42] = 0x05, // B
			[0x43] = 0x06, // C
			[0x44] = 0x07, // D
			[0x45] = 0x08, // E
			[0x46] = 0x09, // F
			[0x47] = 0x0A, // G
			[0x48] = 0x0B, // H
			[0x49] = 0x0C, // I
			[0x4A] = 0x0D, // J
			[0x4B] = 0x0E, // K
			[0x4C] = 0x0F, // L
			[0x4D] = 0x10, // M
			[0x4E] = 0x11, // N
			[0x4F] = 0x12, // O
			[0x50] = 0x13, // P
			[0x51] = 0x14, // Q
			[0x52] = 0x15, // R
			[0x53] = 0x16, // S
			[0x54] = 0x17, // T
			[0x55] = 0x18, // U
			[0x56] = 0x19, // V
			[0x57] = 0x1A, // W
			[0x58] = 0x1B, // X
			[0x59] = 0x1C, // Y
			[0x5A] = 0x1D, // Z

			// Digits
			[0x30] = 0x27, // 0
			[0x31] = 0x1E, // 1
			[0x32] = 0x1F, // 2
			[0x33] = 0x20, // 3
			[0x34] = 0x21, // 4
			[0x35] = 0x22, // 5
			[0x36] = 0x23, // 6
			[0x37] = 0x24, // 7
			[0x38] = 0x25, // 8
			[0x39] = 0x26, // 9

			// Space, Enter, etc.
			[0x20] = 0x2C, // Space
			[0x0D] = 0x28, // Enter
			[0x09] = 0x2B, // Tab
			[0x1B] = 0x29, // Escape
		};

        private static readonly HashSet<int> ModifierVks = new HashSet<int>
        {
			0x12, // VK_MENU (Alt)
			0xA0, // VK_LSHIFT
			0xA1, // VK_RSHIFT
			0xA2, // VK_LCONTROL
			0xA3, // VK_RCONTROL
			0x5B, // VK_LWIN
			0x5C, // VK_RWIN
		};

        private static readonly HashSet<int> MouseButtonVks = new HashSet<int>
        {
            0x01, // VK_LBUTTON
			0x02, // VK_RBUTTON
			0x04, // VK_MBUTTON
			0x05, // VK_XBUTTON1
			0x06, // VK_XBUTTON2
		};

        public int GetDriverKey(int vk)
		{
			return VkToHid.ContainsKey(vk) ? VkToHid[vk] : throw new InvalidOperationException($"No key with vk {vk} found");
		}
        public bool IsKeyboardCharacter(int vk)
        {
			return VkToHid.ContainsKey(vk);
        }

        public bool IsModifierKey(int vk)
        {
            return ModifierVks.Contains(vk);
        }

        public bool IsMouseButton(int vk)
        {
            return MouseButtonVks.Contains(vk);
        }

    }
}
