using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyHandler
{
    public class KeyboardState
    {

        [DllImport("user32.dll")]
        public static extern int GetKeyboardState(byte[] keystate);

        private List<Keys> _keyState;
        private List<Keys> _ignoreList;

        public List<Keys> KeyState
        {
            get
            {
                return _keyState;
            }
        }

        public KeyboardState()
        {
            _ignoreList = new List<Keys>();
            _keyState = new List<Keys>();
        }

        private byte[] GetState()
        {
            byte[] state = new byte[256];
            GetKeyboardState(state);
            return state;
        }

        public void UpdateState()
        {
            _keyState.Clear();
            byte[] keyboardState = GetState();
            for (int i = 0; i < keyboardState.Length; i++)
            {
                if ((keyboardState[i] & 0x80) != 0 && !Enum.IsDefined(typeof(Winuser_Modifiers_Banned), i))
                    if (!_ignoreList.Contains((Keys)i))
                    {
                        _keyState.Add((Keys)i);
                    }
            }
        }

        public void AddIgnoreKey(Keys key)
        {
            _ignoreList.Add(key);
        }

        public static bool IsModifier(Keys key)
        {
            //e.Key != Key.LeftCtrl && e.Key != Key.LeftAlt && e.Key != Key.LeftShift && e.Key != Key.RightCtrl && e.Key != Key.RightAlt && e.Key != Key.RightShift && e.Key != Key.LWin && e.Key != Key.System
            return key == Keys.LControlKey || key == Keys.LMenu || key == Keys.LShiftKey || key == Keys.RControlKey || key == Keys.RMenu || key == Keys.RShiftKey || key == Keys.LWin;
        }

        public static int KeysToModifier(Keys key)
        {
            switch (key)
            {
                case Keys.Alt:
                case Keys.Menu:
                case Keys.LMenu:
                case Keys.RMenu:
                    return 0x0001;

                case Keys.LControlKey:
                case Keys.RControlKey:
                case Keys.Control:
                    return 0x0002;

                case Keys.Shift:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                    return 0x0004;

                case Keys.LWin:
                case Keys.RWin:
                    return 0x0008;
            }
            return 0x0000;
        }

        public static string KeysToString(List<Keys> keys, bool reverse=true)
        {
            List<Keys> keyCopy = new List<Keys>(keys);
            if (reverse)
            {
                keyCopy.Reverse();
            }

            return string.Join(" ", keyCopy);
        }

        public override string ToString()
        {
            return string.Join(" ", _keyState.Reverse<Keys>());
        }
    }
}
