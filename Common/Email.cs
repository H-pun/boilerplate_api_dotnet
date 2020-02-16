using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Common
{
    public class Email
    {
        public static void sendEmail(string email, string subject, string body, bool isHtml = false)
        {
            try
            {
                var smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com", // set your SMTP server name here
                    Port = 587, // Port 
                    EnableSsl = true,
                    Credentials = new NetworkCredential("ischool.service.center@gmail.com", "kosongin1")
                };

                var message = new MailMessage("iSchool Service ischool.service.center@gmail.com", email)
                {
                    IsBodyHtml = isHtml,
                    Subject = subject,
                    Body = body
                };
                {
                    smtpClient.SendMailAsync(message);
                }
            }
            catch { throw; }
        }
    }
}
