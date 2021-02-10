using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WFM.BAL.Helpers
{
    public static class EmailHelper
    {
        public static void SendEmail(MailMessage mailMessage)
        {
            mailMessage.IsBodyHtml = true;

            using (SmtpClient smtp = new SmtpClient
            {
                Host = ResourceData.Host,
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(ResourceData.EmailUser, ResourceData.EmailPassword),
                EnableSsl = true
            })
            {
                smtp.Send(mailMessage);
            }       
        }
    }
}
