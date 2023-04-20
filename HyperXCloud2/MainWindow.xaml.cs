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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;

namespace HyperXCloud2
{
    /// <summary>
    /// </summary>
    public partial class MainWindow : Window
    {
        public Main main;
        public MainWindow()
        {
            InitializeComponent();

            main = new Main();

            Start(0x03F0, 0x0696);
            SetStartup();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Process.GetCurrentProcess().Kill();
        }
        private void SetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (!rk.GetValueNames().Contains("HyperX"))
            {
                rk.SetValue("HyperX", Environment.CurrentDirectory + "//HyperXCloud2.exe");
            }


        }

        private void Start(object sender, RoutedEventArgs e)
        {
            int devices = main.Start(ParseHexStringToInt(Vid.Text), ParseHexStringToInt(Pid.Text));

            DeviceAmount.Text = devices.ToString();

            Active.Text = main.Started.ToString();
        }
        private void Start(int Vid, int Pid)
        {
            SetConfig(Config.LoadConfig());
            Apply();

            main.Start(Vid, Pid);

            Active.Text = main.Started.ToString();
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
            try
            {
                int devices =  main.Start(ParseHexStringToInt(Vid.Text), ParseHexStringToInt(Pid.Text));

                DeviceAmount.Text = devices.ToString();
            }
            catch (Exception ex) {

            }
            if (!main.Started)
            {
                main.Stop();
            }
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
                MediaHandler.VOLUME_AMOUNT = ParseStringToInt(VolumeStep.Text);
            }
            catch (Exception ex) { }


            Config config = new Config(Vid.Text, Pid.Text, Interval.Text, Keycode1.Text, Keycode2.Text, Keycode3.Text, VolumeStep.Text);
            config.SaveConfig();

            Active.Text = main.Started.ToString();
        }
        private void SetConfig(Config config)
        {
            if(config == null) return;

            if(config.variables[0] != null) Vid.Text = config.variables[0];
            if (config.variables[1] != null) Pid.Text = config.variables[1];
            if (config.variables[2] != null) Interval.Text = config.variables[2];
            if (config.variables[3] != null) Keycode1.Text = config.variables[3];
            if (config.variables[4] != null) Keycode2.Text = config.variables[4];
            if (config.variables[5] != null) Keycode3.Text = config.variables[5];
            if (config.variables[6] != null) VolumeStep.Text = config.variables[6];
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
        private byte ParseHexStringToByte(string text)
        {
            byte result = Convert.ToByte(text, 16);
            return result;
        }

        public static Window GetWindow()
        {
            return Window.GetWindow(App.Current.MainWindow) as Window;
        }

    }
}
