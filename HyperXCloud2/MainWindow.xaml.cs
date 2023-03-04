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
            main.Start(Vid, Pid);

            Active.Text = main.Started.ToString();
        }
        private void Stop(object sender, RoutedEventArgs e)
        {
            main.Stop();

            Active.Text = main.Started.ToString();
        }
        private void Stop()
        {
            main.Stop();

            Active.Text = main.Started.ToString();
        }
        private void Apply(object sender, RoutedEventArgs e)
        {
            try
            {
                main.Stop();
                main.Start(ParseHexStringToInt(Vid.Text), ParseHexStringToInt(Pid.Text));
                if (!main.Started)
                {
                    main.Stop();
                }

                MediaHandler.PLAY_PAUSE = ParseHexStringToByte(Keycode1.Text);
                MediaHandler.NEXT = ParseHexStringToByte(Keycode2.Text);
                MediaHandler.PREV = ParseHexStringToByte(Keycode3.Text);

                ClickHandler.ClickInterval = ParseStringToInt(Interval.Text);

            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Active.Text = main.Started.ToString();
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
