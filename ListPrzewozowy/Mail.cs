using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenPop.Pop3;

namespace ListPrzewozowy
{
   public static class Mail
    {
        public static void WyslijEmail(string Message, string file)
        {
            SmtpClient server = new SmtpClient("poczta.o2.pl");
            server.Credentials = new NetworkCredential("test_vir2@o2.pl", "Test1234");
            MailMessage email = new MailMessage();
            email.From = new MailAddress("test_vir2@o2.pl");
            email.To.Add("test.puesc@mf.gov.pl");
            //email.To.Add("sejto@konto.pl");
            email.Subject = "SENT100";
            email.Body = Message;
            string filename = AppDomain.CurrentDomain.BaseDirectory + @"\xml\" + file;
            MessageBox.Show(filename);
            Attachment data = new Attachment(filename, MediaTypeNames.Application.Octet);
            email.Attachments.Add(data);
            try
            {
                server.Send(email);
                MessageBox.Show("Wysłano!");
            }
            catch (SmtpFailedRecipientException error)
            {
                MessageBox.Show("error: " + error.Message + "\nFailing recipient: " + error.FailedRecipient);
            }
        }
    }
}
