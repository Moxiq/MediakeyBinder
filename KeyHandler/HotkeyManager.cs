using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyHandler
{
    public class HotkeyManager
    {
        // DLL libraries used to manage hotkeys
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

         // DLL library to simulate keypress
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);

        private const int WM_HOTKEY = 0x0312;

        private const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const int KEYEVENTF_KEYUP = 0x0002;

        private IntPtr _handle;
        public HotkeyManager(IntPtr hWnd)
        {
            _handle = hWnd;
        }

        // For a full list of modifiers and virtual-key code see: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-registerhotkey
        public bool RegisterCommand(int ID, int fsModifiers, int vlc)
        {
            return RegisterHotKey(_handle, ID, fsModifiers, vlc);
        }

        public bool UnregisterCommand(int ID)
        {
            return UnregisterHotKey(_handle, ID);
        }

        // For a full list of modifiers and virtual-key code see: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-registerhotkey
        public void Keypress(byte keycode)
        {
            keybd_event(keycode, 0, KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
        }

        // Returns key and modifiers (OR:ed together) given a list of keys
        public static (int, int) GetKeyModifiers(List<Keys> keys)
        {
            int key = 0;
            int modifiers = 0;
            foreach (Keys keyStroke in keys)
            {
                if (KeyboardState.IsModifier(keyStroke))
                {
                    int mod = KeyboardState.KeysToModifier(keyStroke);
                    modifiers |= mod;
                }
                else
                {
                    key = (int)keyStroke;
                }
            }
            return (key, modifiers);
        }

    }
}
