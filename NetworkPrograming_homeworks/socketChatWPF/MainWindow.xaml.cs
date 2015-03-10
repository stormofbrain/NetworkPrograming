
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Threading;
using socketChatWPF.Clases;

namespace socketChatWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string _titlePattern = "The Chat [{0}]";
        
        Listener _chListener;
        IPEndPoint _remoteEndPoint;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.Title = String.Format(_titlePattern, "not conected");
            
        }

        private void btnLocalSet_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbxLocalIP.Text))
            {
                MessageBox.Show("Please enter IP address for listener!");
                return;
            }

            int port = 7000;
            int.TryParse(tbxLocalPort.Text, out port);

            _chListener = new Listener(tbxLocalIP.Text, port);
            _chListener.MessageReceived += MessageRecieved;
            _chListener.Start();

            this.Title = String.Format(_titlePattern, _chListener.IPEndPoint.ToString());
        }

        private void btnRemoteSet_Click_1(object sender, RoutedEventArgs e)
        {
            _remoteEndPoint = new IPEndPoint(IPAddress.Parse(tbxRemoteIP.Text), int.Parse(tbxRemotePort.Text));
        }

        private void btnSend_Click_1(object sender, RoutedEventArgs e)
        {
            if (_chListener == null)
            {
                MessageBox.Show("Please, create listener!");
                return;
            }

            if (_remoteEndPoint == null)
            {
                MessageBox.Show("Please, set remote endpoint!");
                return;
            }

            string msg = tbxMessage.Text;
            if (string.IsNullOrEmpty(msg))
            {
                MessageBox.Show("Please, enter message!");
                return;
            }

            tbxHistoryMessage.AppendText(String.Format("[{0}] {1}: ", DateTime.Now.ToString("HH:mm:ss"), tbxNick.Text));
            tbxHistoryMessage.AppendText(msg, Color.FromRgb(0,0,0));
            tbxHistoryMessage.AppendText(Environment.NewLine);

            msg = String.Format("[{0}] {1}: {2}", DateTime.Now.ToString("HH:mm:ss"), tbxNick.Text, msg);
            
            Sender.Send(_remoteEndPoint.Address.ToString(), _remoteEndPoint.Port, msg);
            tbxMessage.Text = "";
        }

        private void tbxMessage_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSend_Click_1(sender, e);
        }
              

        private void MessageRecieved(string msg)
        {
            tbxHistoryMessage.Dispatcher.BeginInvoke(new AddTextDelegate(AddMsg), msg);
        }
        delegate void AddTextDelegate(string msg);
        private void AddMsg(string msg)
        {
            
            tbxHistoryMessage.AppendText(msg);
            tbxHistoryMessage.AppendText(Environment.NewLine);
        }

        private void btnClear_Click_1(object sender, RoutedEventArgs e)
        {
            tbxHistoryMessage.Text = "";
        }

        
    }
    public static class TextBoxExtensions
    {
        public static void AppendText(this TextBox tbx, string text, Color color)
        {
           

            tbx.SelectionStart = tbx.Text.Length;
            tbx.SelectionLength = 0;

            SolidColorBrush myBrush = new SolidColorBrush(color);
            

            tbx.Foreground = myBrush;
            tbx.AppendText(text);
            tbx.Foreground = tbx.Foreground;

            
        }
    }
}
