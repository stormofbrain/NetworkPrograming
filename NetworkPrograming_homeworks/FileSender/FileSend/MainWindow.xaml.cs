using System;
using System.Collections.Generic;
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
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace FileSend
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [Serializable]
        public class FileDatalies
        {
            public string FileType = "";
            public long FileSize = 0;

            public string FILETYPE { get; set; }
            public long FILESIZE { get; set; }
        }

        

        public static FileDatalies fileDet = new FileDatalies();

        private static IPAddress ipAdreses;
        private const int port = 7000;
        private static UdpClient udpClient = new UdpClient();
        private static IPEndPoint ipEndPoint;

        private static FileStream fs;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSend_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                ipAdreses = IPAddress.Parse(tbxIP.Text);
                ipEndPoint = new IPEndPoint(ipAdreses, port);
                fs = new FileStream(@tbxPath.Text, FileMode.Open, FileAccess.Read);

                if (fs.Length > 8192)
                {
                    MessageBox.Show("File size biggest than 8kb");
                    udpClient.Close();
                    fs.Close();
                    return;
                }

                SendFileInfo();

                Thread.Sleep(1000);

                SendFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void SendFileInfo()
        {
            fileDet.FILETYPE = fs.Name.Substring((int)fs.Name.Length - 3 , 3);
            fileDet.FILESIZE = fs.Length;

            XmlSerializer fileSerializer = new XmlSerializer(typeof(FileDatalies));
            MemoryStream memoryStream = new MemoryStream();

           fileSerializer.Serialize(memoryStream, fileDet);
          
                
          

            memoryStream.Position = 0;
            Byte[] bytes = new Byte[memoryStream.Length];
            memoryStream.Read(bytes, 0, Convert.ToInt32(memoryStream.Length));
            MessageBox.Show("Send File Info");

            udpClient.Send(bytes, bytes.Length, ipEndPoint);
            MessageBox.Show(bytes[0] + " " +bytes.Length + " " + ipEndPoint);
            memoryStream.Close();

        }
        private static void SendFile()
        {
            Byte[] bytes = new Byte[fs.Length];

            fs.Read(bytes, 0, bytes.Length);
            MessageBox.Show("Send file with size " + fs.Length + "bytes");
            try
            {
                udpClient.Send(bytes, bytes.Length, ipEndPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                fs.Close();
                udpClient.Close();
            }
            MessageBox.Show("File Sended");
        }
        
       
    }
}
