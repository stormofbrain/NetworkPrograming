using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace socketChatWPF.Clases
{
    public class Sender
    {
        public static void Send(string ip, int port, string message)
        {
            try
            {
                var ipAddress = IPAddress.Parse(ip);
                var ipEndPoint = new IPEndPoint(ipAddress, port);

                Socket Connector = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Connector.Connect(ipEndPoint);
                Byte[] SendBytes = Encoding.UTF8.GetBytes(message);
                Connector.Send(SendBytes);
                Connector.Close();
            }
            catch (Exception ex)
            {
                int t = 2;
            }
        }
    }
}
