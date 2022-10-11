using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace Intermediario.MailService
{
    public class ServidorCorreos
    {

        private SmtpClient smtpClient;
        protected string senderMail { get; set; }
        protected string password { get; set; }
        protected string host { get; set; }
        protected int port { get; set; }
        protected bool ssl { get; set; }

        protected void initializaSetpCliente()
        {
            smtpClient = new SmtpClient();
            smtpClient.Credentials = new NetworkCredential(senderMail, password);
            smtpClient.Host = host;
            smtpClient.Port = port;
            smtpClient.EnableSsl = ssl;

        }

        public void sendMail(string subject, string body, string recipientMail)
        {
            var mailMessage = new MailMessage();

            try
            {
                mailMessage.From = new MailAddress(senderMail);
                mailMessage.To.Add(recipientMail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.Priority = MailPriority.Normal;
                smtpClient.Send(mailMessage);
            }
            catch{ }
            finally
            {
                mailMessage.Dispose();
               //smtpClient.Dispose();
            }
            
        }


    }
}
