using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Window = System.Windows.Window;



namespace WBR
{
    public partial class MainWindow : Window
    {

        public Main Main;
        private NotifyIcon TrayIcon;
        private WindowState StoredWindowState = WindowState.Normal;
        public static Config Config = new Config();
        private static DebugWindow debugWindow = null;
        public MainWindow()
        {
            ErrorHandler.NewError("Starting");
            InitializeComponent();

            Main = new Main();
            SetupTray();
            Start();
            SetStartup();



        }
        // Start
        private void Start(object sender, RoutedEventArgs e)
        {
            Start();
        }
        private void Start()
        {
            DevicePresets.Init();
            RefreshComboBox();
            Config.LoadConfig();
            ApplyConfig();
            Apply();

            Active.Text = Main.Started.ToString();
        }

        private void RefreshComboBox()
        {
            DeviceName.Items.Clear();
            bool first = true;
            foreach (var pair in DevicePresets.Presets)
            {
                DeviceName.Items.Add(new ComboBoxItem { Content = pair.Key, IsSelected = first });
                if (first) first = false;
            }

        }

        // Apply
        private void Apply(object sender, RoutedEventArgs e)
        {
            Apply();
        }


        private void Apply()
        {
            Main.Stop();
            if (!Main.Started)
            {
                Main.Stop();
            }

            Main.Start(GetDeviceName(), Config.VendorID, Config.ProductID);

            MediaHandler.PLAY_PAUSE = ErrorHandler.Try(ParseHexStringToByte, Keycode1.Text);
            MediaHandler.NEXT = ErrorHandler.Try(ParseHexStringToByte, Keycode2.Text);
            MediaHandler.PREV = ErrorHandler.Try(ParseHexStringToByte, Keycode3.Text);
            ClickHandler.ClickInterval = ErrorHandler.Try(ParseStringToInt, Interval.Text);

            Config.Keycode1 = MediaHandler.PLAY_PAUSE;
            Config.Keycode2 = MediaHandler.NEXT;
            Config.Keycode3 = MediaHandler.PREV;
            Config.Interval = ClickHandler.ClickInterval;

            if (HideTray.IsChecked != null)
                Config.ShouldHideInTray = (bool)HideTray.IsChecked;

            TrayIcon.Visible = Config.ShouldHideInTray;

            Config.VendorID = ErrorHandler.Try(ParseHexStringToInt, Vid.Text);
            Config.ProductID = ErrorHandler.Try(ParseHexStringToInt, Pid.Text);
            Config.Device = GetDeviceName();
            Config.SaveConfig();

            Active.Text = Main.Started.ToString();
        }
        private void Update()
        {
            ApplyConfig();
            Apply();
        }
        private void ApplyConfig()
        {
            RefreshComboBox();
            Vid.Text = Config.VendorID.ToString("X");
            Pid.Text = Config.ProductID.ToString("X");
            Interval.Text = Config.Interval.ToString();
            Keycode1.Text = Config.Keycode1.ToString("X");
            Keycode2.Text = Config.Keycode2.ToString("X");
            Keycode3.Text = Config.Keycode3.ToString("X");
            var items = DeviceName.Items;
            int i;
            for (i = 0; i < items.Count; i++)
            {
                DeviceName.SelectedIndex = i;
                if (GetDeviceName() == Config.Device)
                    break;
            }
            DeviceName.SelectedIndex = i;
            HideTray.IsChecked = Config.ShouldHideInTray;
            Active.Text = Main.Started.ToString();
        }



        private void TrayIconClick(object sender, EventArgs e)
        {
            Show();
            WindowState = StoredWindowState;
            Activate();
        }
        protected override void OnClosed(EventArgs e)
        {
            TrayIcon.Visible = false;
            TrayIcon.Dispose();
            base.OnClosed(e);
            Process.GetCurrentProcess().Kill();
        }
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized && Config.ShouldHideInTray) this.Hide();

            //base.OnStateChanged(e);
        }
        private void SetStartup()
        {
            const string appName = "WBR";

            using var key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run",
                writable: true);

            if (key == null)
                throw new InvalidOperationException("Unable to access startup registry key.");

            string exePath = Environment.ProcessPath
                ?? throw new InvalidOperationException("Unable to determine executable path.");

            key.SetValue(appName, $"\"{exePath}\"");

        }

        private void AddRegistryEntry(string name, string path)
        {
            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (!rk.GetValueNames().Contains(name))
                {
                    rk.SetValue(name, path);
                }
            }
            catch { } // prob no admin priv given, ignore
        }

        private void SetupTray()
        {
            TrayIcon = new NotifyIcon();

            TrayIcon.BalloonTipText = "";
            TrayIcon.BalloonTipTitle = "WBR";
            TrayIcon.Visible = true;
            TrayIcon.Text = "WBR";
            TrayIcon.Icon = new System.Drawing.Icon(FileHandler.EnvironmentPath + "icon.ico");

            TrayIcon.Click += new EventHandler(TrayIconClick);
            StoredWindowState = WindowState;
        }
        private void Stop(object sender, RoutedEventArgs e)
        {
            Stop();
        }
        private void Stop()
        {
            Main.Stop();

            Active.Text = Main.Started.ToString();
        }

        private void Settings(object sender, RoutedEventArgs e)
        {
            if (debugWindow == null || !debugWindow.IsLoaded || debugWindow.Visibility != Visibility.Visible)
                debugWindow = new DebugWindow(Update, ref Config);

            debugWindow.Show();
            if (debugWindow != null && debugWindow.IsVisible)
            {
                debugWindow.Activate();
                return;
            }
        }

        private int ParseStringToInt(string text)
        {
            return ErrorHandler.Try(int.Parse, text);

        }
        private int ParseHexStringToInt(string text)
        {
            int result = int.Parse(text, System.Globalization.NumberStyles.HexNumber);
            return result;
        }
        private bool ParseStringToBool(string text)
        {
            return bool.Parse(text);
        }
        private byte ParseHexStringToByte(string text)
        {
            byte result = Convert.ToByte(text, 16);
            return result;
        }

        public static Window GetWindow()
        {
            return Window.GetWindow(App.Current.MainWindow) as Window;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private string GetDeviceName()
        {
            return DeviceName.SelectedValue.ToString();
        }

        private void HideTray_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
