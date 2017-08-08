using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace EmployeeInformationSystem.Web.Helpers
{
    public class EmailSender
    {
        public static string smtpServer = WebConfigurationManager.AppSettings["smtpServer"].ToString();
        public static string smtpPort = WebConfigurationManager.AppSettings["smtpPort"].ToString();
        public static string smtpUser = WebConfigurationManager.AppSettings["smtpUser"].ToString();
        public static string smtpPass = WebConfigurationManager.AppSettings["smtpPass"].ToString();
        public static List<string> smtpTo = new List<string>() { WebConfigurationManager.AppSettings["smtpTo"].ToString() };

        public static bool Send(string subject, string body)
        {
            var _mailMsg = new MailMessage();
            _mailMsg.From = new MailAddress(smtpUser);

            _mailMsg.To.Add(string.Join(",", smtpTo.ToArray()));
            var _mailBody = new StringBuilder();
            _mailBody.Append(body);

            _mailMsg.IsBodyHtml = true;
            _mailMsg.Subject = subject;
            _mailMsg.Body = _mailBody.ToString();

            SmtpClient smtpClient = new SmtpClient(smtpServer, int.Parse(smtpPort));
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = smtpUser,
                Password = smtpPass
            };

            try
            {
                smtpClient.EnableSsl = true;
                smtpClient.Send(_mailMsg);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool Send(string subject, string body, List<string> to)
        {
            var _mailMsg = new MailMessage();
            _mailMsg.From = new MailAddress(smtpUser);

            _mailMsg.To.Add(string.Join(",", to.ToArray()));
            var _mailBody = new StringBuilder();
            _mailBody.Append(body);

            _mailMsg.IsBodyHtml = true;
            _mailMsg.Subject = subject;
            _mailMsg.Body = _mailBody.ToString();

            SmtpClient smtpClient = new SmtpClient(smtpServer, int.Parse(smtpPort));
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = smtpUser,
                Password = smtpPass
            };

            try
            {
                smtpClient.EnableSsl = true;
                smtpClient.Send(_mailMsg);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

