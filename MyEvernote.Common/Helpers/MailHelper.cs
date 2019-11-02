using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Common.Helpers
{
    public class MailHelper
    {
        //Tek adres alan metodum.
        public static bool SendMail(string body, string to, string subject, bool isHtml = true)
        {
            return SendMail(body, new List<string> { to }, subject, isHtml);
        }

        public static bool SendMail(string body, List<string> to, string subject, bool isHtml = true)
        {
            bool result = false;

            try
            {
                var message = new MailMessage(); 
                message.From = new MailAddress(ConfigHelper.Get<string>("MailUser")); //Confighelperda get metodunda git mail useri oku dedim. Webconfig appsetting

                to.ForEach(x =>
                {
                    message.To.Add(new MailAddress(x)); //yazdığımız adresler tek tek eklenecek.
                });

                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;

                using (var smtp = new SmtpClient(
                    ConfigHelper.Get<string>("MailHost"), //sunucum
                    ConfigHelper.Get<int>("MailPort"))) //kullanacağım port .. confighelperi kullanarak appsettingsten alacak
                {
                    smtp.EnableSsl = true;
                    smtp.Credentials =
                        new NetworkCredential(
                            ConfigHelper.Get<string>("MailUser"),  //bu kullanıcı adı
                            ConfigHelper.Get<string>("MailPass")); //bu şifreyle gönder

                    smtp.Send(message);
                    result = true;
                }
            }
            catch (Exception)
            {

            }

            return result;
        }
    }
}