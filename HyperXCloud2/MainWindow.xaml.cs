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

namespace HyperXCloud2
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Main main;
        public MainWindow()
        {
            InitializeComponent();

            main = new Main();

            Start(0x03F0, 0x0696);
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Process.GetCurrentProcess().Kill();
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            main.Start(ParseHexStringToInt(Vid.Text), ParseHexStringToInt(Pid.Text));

            Active.Text = main.Started.ToString();
        }
        private void Start(int Vid, int Pid)
        {
            SetConfig(FileHandler.LoadConfig());
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
                main.Start(ParseHexStringToInt(Vid.Text), ParseHexStringToInt(Pid.Text));
            }
            catch (Exception ex) { }
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


            Config config = new Config(Vid.Text, Pid.Text, Interval.Text, Keycode1.Text, Keycode2.Text, Keycode3.Text);
            FileHandler.SaveConfig(config);

            Active.Text = main.Started.ToString();
        }
        private void SetConfig(Config config)
        {
            if(config == null) return;

            Vid.Text = config.variables[0]; Pid.Text = config.variables[1]; Interval.Text = config.variables[2]; Keycode1.Text = config.variables[3]; Keycode2.Text = config.variables[4]; Keycode3.Text = config.variables[5];
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
    }
}
