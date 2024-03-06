using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Helpers
{
    /// <summary>
    /// 寄送郵件相關的工具函式
    /// </summary>
    public static class MailHelper
    {
        //透過SMTP寄送電子郵件
        public static void SendMail(string from, string to, string subject, string body, string smtpServer, string smtpPort, string smtpUser, string smtpPassword)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.From = new System.Net.Mail.MailAddress(from);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(smtpServer, Convert.ToInt32(smtpPort));
            smtp.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPassword);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}
