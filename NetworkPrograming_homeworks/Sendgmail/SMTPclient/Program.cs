using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipAddr = Dns.GetHostAddresses("localhost")[1];

        
            
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("unixvb", "vekTRA152");
            smtpClient.EnableSsl = true;
            try
            {
                MailMessage message = new MailMessage();
                message.To.Add("serhiyroiiko96@gmail.com ");
                message.From = new MailAddress("festivall.pr@gmail.com");
                message.Subject = "test";
                message.Body = "NIGGA!";
                smtpClient.Send(message);
                message = null;
                Console.WriteLine("Message sent\n");
            }
            catch( Exception ex )
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
