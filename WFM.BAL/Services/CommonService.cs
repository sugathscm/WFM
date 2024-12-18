using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Configuration;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using WFM.DAL;
using System.Configuration;
using System.Web.Mail;
using MailMessage = System.Net.Mail.MailMessage;

namespace WFM.BAL.Services
{
    public static class CommonService
    {
        public static DateTime GetClientDate(HttpRequestBase request  )
        {
            //string IP = request.ServerVariables["REMOTE_ADDR"];
            //XmlDocument doc = new XmlDocument();
            //doc.Load("http://www.showmyip.com/xml/?ip=" + IP);
            //XmlNodeList nodeLstCountry = doc.GetElementsByTagName("lookup_country");

            //var country = nodeLstCountry[0].InnerText;

            return DateTime.UtcNow.AddHours(5).AddMinutes(30);
        }

        public static int SaveLoginAudit(LoginAudit loginAudit)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                entities.LoginAudits.Add(loginAudit);
                entities.SaveChanges();
            }
            return 1;
        }

        public static int SaveDataAudit(DataAudit dataAudit)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                entities.DataAudits.Add(dataAudit);
                entities.SaveChanges();
            }
            return 1;
        }

        public static List<DataAudit> GetDataAuditByUser(Guid userId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.DataAudits.Include("").Where(l => l.UserId == userId).OrderByDescending(o => o.UpdatedOn).ToList();
            }
        }

        public static List<LoginAudit> GetLoginAuditByUser(Guid userId)
        {
            using (LinkManagementEntities entities = new LinkManagementEntities())
            {
                return entities.LoginAudits.Where(l => l.UserId == userId).OrderByDescending(o => o.DateLogged).ToList();
            }
        }

        public static T Cast<T>(this Object myobj)
        {
            Type objectType = myobj.GetType();
            Type target = typeof(T);
            var x = Activator.CreateInstance(target, false);
            var z = from source in objectType.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            var d = from source in target.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
               .ToList().Contains(memberInfo.Name)).ToList();
            PropertyInfo propertyInfo;
            object value;
            foreach (var memberInfo in members)
            {
                try
                {
                    propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                    value = myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null);

                    propertyInfo.SetValue(x, value, null);
                }
                catch (Exception ex)
                {
                    string error = ex.ToString();
                }
 
            }
            return (T)x;
        }

        public static int SendEmail(SmtpSection smtpSection, string To, string CC, string BCC, string Subject, string Body)
        {
            smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(smtpSection.From);
                mail.To.Add(To);
                string[] CCId = CC.Split(',');
                foreach (string CCEmail in CCId)
                {
                    mail.CC.Add(new MailAddress(CCEmail));  
                }
                mail.Bcc.Add(BCC);
                mail.Subject = Subject;
                mail.Body = Body;

                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("C:\\Users\\Suga\\Downloads\\BIADP - Phase II Stage II.pdf")); //Server.MapPath(model.Attachment)));

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }

                return 1;
            }
        }

        public static int SendEmail(SmtpSection smtpSection, string To, string CC, string Subject, string Body)
        {
            MailAddressCollection mailAddressCollection = new MailAddressCollection();

            smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(smtpSection.From);
                mail.To.Add(To);
                //mail.CC.Add(CC);
                mail.Bcc.Add(CC);
                mail.Subject = Subject;
                mail.Body = Body;

                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("C:\\Users\\Suga\\Downloads\\BIADP - Phase II Stage II.pdf")); //Server.MapPath(model.Attachment)));

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }

                return 1;
            }
        }
        public static int SendEmail(SmtpSection smtpSection, string To, string Subject, string Body)
        {
            MailAddressCollection mailAddressCollection = new MailAddressCollection();

            smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(smtpSection.From);
                mail.To.Add(To);
                //mail.CC.Add(CC);
                //mail.Bcc.Add(CC);
                mail.Subject = Subject;
                mail.Body = Body;

                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("C:\\Users\\Suga\\Downloads\\BIADP - Phase II Stage II.pdf")); //Server.MapPath(model.Attachment)));

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }


                return 1;
            }
        }

    }
}
