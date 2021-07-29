using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeyHandler;

namespace MediaKeyBinder
{
    public class Userdata
    {

        public List<Keys> VolumeUp { get; set; }
        public List<Keys> VolumeDown { get; set; }
        public List<Keys> NextTrack { get; set; }
        public List<Keys> PrevTrack {get; set; }
        public List<Keys> ToggleMute {get; set; }
        public List<Keys> PlayPause {get; set; }

        public void UpdateValue(Hotkey_IDS keyID, List<Keys> keys)
        {
            switch (keyID)
            {
                case Hotkey_IDS.VolumeUp:
                    VolumeUp = new List<Keys>(keys);
                    break;
                case Hotkey_IDS.VolumeDown:
                    VolumeDown = new List<Keys>(keys);
                    break;
                case Hotkey_IDS.NextTrack:
                    NextTrack = new List<Keys>(keys);
                    break;
                case Hotkey_IDS.PrevTrack:
                    PrevTrack = new List<Keys>(keys);
                    break;
                case Hotkey_IDS.ToggleMute:
                    ToggleMute = new List<Keys>(keys);
                    break;
                case Hotkey_IDS.PlayPause:
                    PlayPause = new List<Keys>(keys);
                    break;
            }
        }
    }


}
