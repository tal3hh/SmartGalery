using DomainLayer.Entities;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using ServiceLayer.Services.Interfaces;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ServiceLayer.Services
{
    public class MessageSend : IMessageSend
    {
        public void MimeKitConfrim(AppUser appUser, string url, string token)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("SmartGalery", "projectogani@gmail.com"));

            message.To.Add(new MailboxAddress(appUser.UserName, appUser.Email));

            message.Subject = "Confirm Email";

            //string emailbody = string.Empty;

            //using (StreamReader streamReader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Account/Templates", "Confirm.html")))
            //{
            //    emailbody = streamReader.ReadToEnd();
            //}

            //emailbody = emailbody.Replace("{{username}}", $"{appUser.UserName}").Replace("{{code}}", $"{url}");

            //message.Body = new TextPart(TextFormat.Html) { Text = emailbody };

            string emailBody = "<h2> Emaili Tesdiq Edin <h2> <hr>";

            emailBody += $"<h5><a href='{url}'> bu linke klik edin. </a></h5>";

            message.Body = new TextPart(TextFormat.Html) { Text = emailBody };

            using var smtp = new SmtpClient();

            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("projectogani@gmail.com", "cjywfdxcacwbtixw");
            smtp.Send(message);
            smtp.Disconnect(true);
        }


        public void MimeMessageResetPassword(AppUser user, string url, string code)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("SmartGalery", "projectogani@gmail.com"));

            message.To.Add(new MailboxAddress(user.UserName, user.Email));

            message.Subject = "Reset Password";

            //string emailbody = string.Empty;

            //using (StreamReader streamReader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Account/Templates", "Confirm.html")))
            //{
            //    emailbody = streamReader.ReadToEnd();
            //}

            //emailbody = emailbody.Replace("{{username}}", $"{user.UserName}").Replace("{{code}}", $"{url}");

            //message.Body = new TextPart(TextFormat.Html) { Text = emailbody };

            string emailBody = "<h2> Emaili Tesdiq Edin <h2> <hr>";

            emailBody += $"<h5><a href='{url}'> bu linke klik edin. </a></h5>";

            message.Body = new TextPart(TextFormat.Html) { Text = emailBody };

            using var smtp = new SmtpClient();

            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("projectogani@gmail.com", "cjywfdxcacwbtixw");
            smtp.Send(message);
            smtp.Disconnect(true);
        }
    }
}
