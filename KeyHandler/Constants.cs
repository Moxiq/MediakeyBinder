using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyHandler
{
    public enum WM_Modifiers
    {
        None = 0x0000,
        Alt = 0x0001,
        Ctrl = 0x0002,
        Shift = 0x0004,
        Win = 0x0008
    }

    public enum Winuser_Modifiers_Banned
    {
        //VK_LSHIFT = 0xA0,
        //VK_RSHIFT = 0xA1,
        //VK_LCONTROL = 0xA2,
        //VK_RCONTROL = 0xA3,
        //VK_LMENU = 0xa4,
        //VK_RMENU = 0xA5
        VK_SHIFT = 0x10,
        VK_CONTROL = 0x11,
        VK_MENU = 0x12
    }

    public enum Hotkey_IDS
    {
        VolumeUp,
        VolumeDown,
        NextTrack,
        PrevTrack,
        ToggleMute,
        PlayPause,
        Invalid
    }

    public enum Mediakeys
    {
        VK_VOLUME_UP = 0xAF,
        VK_VOLUME_DOWN = 0xAE,
        VK_MEDIA_NEXT_TRACK = 0xB1,
        VK_MEDIA_PREV_TRACK = 0xB1,
        VK_MEDIA_PLAY_PAUSE = 0xB3,
        VK_VOLUME_MUTE = 0xAD

    }
}
