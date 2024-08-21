using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;

namespace WBR
{
    /// <summary>
    /// </summary>
    public partial class MainWindow : Window
    {

        public Main main;
        private NotifyIcon trayIcon;
        private WindowState storedWindowState = WindowState.Normal;
        private bool ShouldHideInTray = false;
        public MainWindow()
        {
            InitializeComponent();

            main = new Main();
            Start();
            SetStartup();
        }
        private void TrayIconClick(object sender, EventArgs e)
        {
            Show();
            WindowState = storedWindowState;
            Activate();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Process.GetCurrentProcess().Kill();
        }
        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized && ShouldHideInTray) this.Hide();

            //base.OnStateChanged(e);
        }
        private void SetStartup()
        {


            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

           // if (!rk.GetValueNames().Contains("WBR"))
            {
                rk.SetValue("WBR", Environment.CurrentDirectory + "\\WBR.exe");
            }

            rk = Registry.LocalMachine.OpenSubKey
            ("SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            // if (!rk.GetValueNames().Contains("WBR"))
            {
                rk.SetValue("WBR", Environment.CurrentDirectory + "\\WBR.exe");
            }

            rk = Registry.LocalMachine.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\run", true);

            // if (!rk.GetValueNames().Contains("WBR"))
            {
                rk.SetValue("WBR", Environment.CurrentDirectory + "\\WBR.exe");
            }


        }
        
        private void Start(object sender, RoutedEventArgs e)
        {
            Start();
        }
        private void Start()
        {
            SetupTray();

            SetConfig(Config.LoadConfig());
            Apply();

            Active.Text = main.Started.ToString();
        }

        private void SetupTray()
        {
            trayIcon = new System.Windows.Forms.NotifyIcon();

            trayIcon.BalloonTipText = "";
            trayIcon.BalloonTipTitle = "WBR";
            trayIcon.Visible = true;
            trayIcon.Text = "WBR";
            trayIcon.Icon = new System.Drawing.Icon(FileHandler.EnvironmentPath + "icon.ico");

            trayIcon.Click += new EventHandler(TrayIconClick);
            storedWindowState = WindowState;
        }
        private void Stop(object sender, RoutedEventArgs e)
        {
            Stop();
        }
        private void Stop()
        {
            main.Stop();

            Active.Text = main.Started.ToString();
        }
        private void Apply(object sender, RoutedEventArgs e)
        {
            Apply();
        }
        private void Apply()
        {

            main.Stop();
            if (!main.Started)
            {
                main.Stop();
            }
            try
            {
                int devices =  main.Start(GetDeviceName(), ParseHexStringToInt(Vid.Text), ParseHexStringToInt(Pid.Text));

                DeviceAmount.Text = devices.ToString();
            }
            catch (Exception ex) {}
            try
            {
                MediaHandler.PLAY_PAUSE = ParseHexStringToByte(Keycode1.Text);
            }catch (Exception ex) { }
            try
            {
                MediaHandler.NEXT = ParseHexStringToByte(Keycode2.Text);
            }catch (Exception ex) { }
            try
            {
                MediaHandler.PREV = ParseHexStringToByte(Keycode3.Text);
            } catch (Exception ex) { }

            try
            {
                ClickHandler.ClickInterval = ParseStringToInt(Interval.Text);
            }catch (Exception ex) { }

            try
            {
                //MediaHandler.VOLUME_AMOUNT = ParseStringToInt(VolumeStep.Text);
            }
            catch (Exception ex) { }

            try
            {
                bool? b = HideTray.IsChecked;
                if(b != null)
                {
                    ShouldHideInTray = (bool)b;
                }
            }
            catch (Exception ex) { }


            Config config = new Config(Vid.Text, Pid.Text, Interval.Text, Keycode1.Text, Keycode2.Text, Keycode3.Text, "Device", ShouldHideInTray);
            config.SaveConfig();

            Active.Text = main.Started.ToString();
        }
        private void SetConfig(Config config)
        {

            if (config == null) return;

            if(config.variables[0] != null) Vid.Text = config.variables[0];
            if (config.variables[1] != null) Pid.Text = config.variables[1];
            if (config.variables[2] != null) Interval.Text = config.variables[2];
            if (config.variables[3] != null) Keycode1.Text = config.variables[3];
            if (config.variables[4] != null) Keycode2.Text = config.variables[4];
            if (config.variables[5] != null) Keycode3.Text = config.variables[5];
            //if (config.variables[6] != null) VolumeStep.Text = config.variables[6];

            // TODO: error checking? lets hope this wont throw any errors :)
            if (config.variables[7] != null) HideTray.IsChecked = ParseStringToBool(config.variables[7]);
        }
        private int ParseStringToInt(string text)
        {
            int result = int.Parse(text);
            return result;
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
            Console.WriteLine(DeviceName.SelectedValue.ToString());
            return DeviceName.SelectedValue.ToString();
        }
    }
}
