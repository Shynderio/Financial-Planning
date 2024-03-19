﻿using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Data.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        public void SendWelcomeEmail(string Username, string Password, string emailAddress, string createdUser)
        {

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Financial Planning System", "noreply@yourdomain.com"));
            email.To.Add(new MailboxAddress("", emailAddress));
            email.Subject = "no-reply-email-Financial Planning-system" + "<" + createdUser + ">";
            email.Body = new TextPart("html")
            {
                Text =
            $"This email is from Financial Planning System.<br/><br/><br/>" +
            $"User name: <span style='color: red; '>{Username}</span><br/>" +
            $"Password: <span style='color: red;'>{Password}</span><br/><br/><br/><br/><br/><br/></p>" +
            $"<p><span style='color: green;'>If anything wrong, please contact to 012345678. We are so sorry for this inconvenience. <br/><br/><br/>" +
            $"Thanks & Regards! " +
            $"Financial Planning Team.</span></p>"
            };

            // usinh SMTP client to send email
            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("tuanmin18062003@gmail.com", "zmuf vsoq okzg xluw");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}