using KeyHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.Json;
using TextBox = System.Windows.Controls.TextBox;
using FileHandler;
using System.ComponentModel;

namespace MediaKeyBinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HotkeyManager _keyHandler;
        private KeyboardState _keyboardState;
        private Userdata _userdata;
        private NotifyIcon _notifyIcon;

        private const string DATAPATH = "Keybinds.json";

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _notifyIcon = new NotifyIcon
            {
                BalloonTipText = "The app has been minimised. Click the tray icon to show.",
                BalloonTipTitle = "Media Key Binder",
                Text = "Media Key Binder",
                Icon = Properties.Resources.speaker
            };
            _notifyIcon.Click += new EventHandler(_notifyIcon_Click);

            IntPtr handle = new WindowInteropHelper(this).Handle;
            HwndSource source = HwndSource.FromHwnd(handle);
            source.AddHook(HwndHook);
            _userdata = new Userdata();
            _keyHandler = new HotkeyManager(handle);
            _keyboardState = new KeyboardState();
            _keyboardState.AddIgnoreKey(Keys.VolumeUp);
            _keyboardState.AddIgnoreKey(Keys.VolumeDown);
            _keyboardState.AddIgnoreKey(Keys.MediaNextTrack);
            _keyboardState.AddIgnoreKey(Keys.MediaPreviousTrack);
            _keyboardState.AddIgnoreKey(Keys.MediaPlayPause);
            _keyboardState.AddIgnoreKey(Keys.MediaStop);
            _keyboardState.AddIgnoreKey(Keys.VolumeMute);

            RegisterUserData(DATAPATH);
        }

        private void SaveHotkey(Hotkey_IDS id, List<Keys> keys)
        {
            (int key, int modifiers) = HotkeyManager.GetKeyModifiers(keys);
            if (!_keyHandler.RegisterCommand((int)id, modifiers, key))
            {
                Console.WriteLine("Could not register hotkey");
            }
            else
            {
                _userdata.UpdateValue(id, keys);
                JsonHandler.Serialize(DATAPATH, _userdata);
            }
        }

        // How can this be handled better?
        private void RegisterUserData(string filepath)
        {
            try
            {
                Userdata data = JsonHandler.Deserialize<Userdata>(DATAPATH);

                if (data.VolumeUp != null)
                {
                    SaveHotkey(Hotkey_IDS.VolumeUp, data.VolumeUp);
                    txt_Vol_Up.Text = KeyboardState.KeysToString(data.VolumeUp);
                }

                if (data.VolumeDown != null)
                {
                    SaveHotkey(Hotkey_IDS.VolumeDown, data.VolumeDown);
                    txt_Vol_Down.Text = KeyboardState.KeysToString(data.VolumeDown);
                }

                if (data.NextTrack != null)
                {
                    SaveHotkey(Hotkey_IDS.NextTrack, data.NextTrack);
                    txt_Next.Text = KeyboardState.KeysToString(data.NextTrack);
                }

                if (data.PrevTrack != null)
                {
                    SaveHotkey(Hotkey_IDS.PrevTrack, data.PrevTrack);
                    txt_Prev.Text = KeyboardState.KeysToString(data.PrevTrack);
                }

                if (data.ToggleMute != null)
                {
                    SaveHotkey(Hotkey_IDS.ToggleMute, data.ToggleMute);
                    txt_Mute.Text = KeyboardState.KeysToString(data.ToggleMute);
                }

                if (data.PlayPause != null)
                {
                    SaveHotkey(Hotkey_IDS.PlayPause, data.PlayPause);
                    txt_PlayPause.Text = KeyboardState.KeysToString(data.PlayPause);
                }
            }
            catch (FileNotFoundException e)
            {
                // Don't have to address this, file is created later automatically. 
            }
        }

        private Hotkey_IDS GetHotkeyId(object control)
        {
            if (control.GetType() == typeof(TextBox))
            {
                TextBox txt = (TextBox)control;
                switch (txt.Name)
                {
                    case "txt_Vol_Up":
                        return Hotkey_IDS.VolumeUp;
                    case "txt_Vol_Down":
                        return Hotkey_IDS.VolumeDown;
                    case "txt_Next":
                        return Hotkey_IDS.NextTrack;
                    case "txt_Prev":
                        return Hotkey_IDS.PrevTrack;
                    case "txt_Mute":
                        return Hotkey_IDS.ToggleMute;
                    case "txt_PlayPause":
                        return Hotkey_IDS.PlayPause;
                    default:
                        break;
                }
            }
            return Hotkey_IDS.Invalid;
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;

            if (msg == WM_HOTKEY)
            {
                switch ((int)wParam)
                {
                    case (int)Hotkey_IDS.VolumeUp:
                        _keyHandler.Keypress((byte)Mediakeys.VK_VOLUME_UP);
                        break;

                    case (int)Hotkey_IDS.VolumeDown:
                        _keyHandler.Keypress((byte)Mediakeys.VK_VOLUME_DOWN);
                        break;

                    case (int)Hotkey_IDS.NextTrack:
                        _keyHandler.Keypress((byte)Mediakeys.VK_MEDIA_NEXT_TRACK);
                        break;

                    case (int)Hotkey_IDS.PrevTrack:
                        _keyHandler.Keypress((byte)Mediakeys.VK_MEDIA_PREV_TRACK);
                        break;
                    case (int)Hotkey_IDS.ToggleMute:
                        _keyHandler.Keypress((byte)Mediakeys.VK_VOLUME_MUTE);
                        break;

                    case (int)Hotkey_IDS.PlayPause:
                        _keyHandler.Keypress((byte)Mediakeys.VK_MEDIA_PLAY_PAUSE);
                        break;
                    default:
                        break;
                }
            }
            return IntPtr.Zero;
        }

        private void OnFocusTxt(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = "Press a key";
        }

        private void OnLostFocusTxt(object sender, RoutedEventArgs e)
        {
        }

        private void OnPreviewKeyDownTxt(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!e.IsRepeat)
            {
                TextBox txtBox = (TextBox)sender;
                _keyboardState.UpdateState();

                if (e.Key != Key.LeftCtrl && e.Key != Key.LeftAlt && e.Key != Key.LeftShift && e.Key != Key.RightCtrl && e.Key != Key.RightAlt && e.Key != Key.RightShift && e.Key != Key.LWin && e.Key != Key.System) // Remake this
                {
                    Keyboard.ClearFocus();

                    int id = (int)GetHotkeyId(sender);
                    SaveHotkey((Hotkey_IDS)id, _keyboardState.KeyState);

                }
                txtBox.Clear();
                txtBox.Text = _keyboardState.ToString();
            }

        }

        private void OnClose(object sender, CancelEventArgs args)
        {
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        private WindowState m_storedWindowState = WindowState.Normal;
        private void OnStateChanged(object sender, EventArgs args)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                if (_notifyIcon != null)
                {
                    _notifyIcon.ShowBalloonTip(2000);
                }
            }
            else
            {
                m_storedWindowState = WindowState;
            }
                
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            CheckTrayIcon();
        }

        private void _notifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = m_storedWindowState;
        }

        void CheckTrayIcon()
        {
            ShowTrayIcon(!IsVisible);
        }

        void ShowTrayIcon(bool show)
        {
            if (_notifyIcon != null)
                _notifyIcon.Visible = show;
        }
    }
}
