using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.LLB.Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    "s12216872@stu.najah.edu",
                    "epwg ltux qwfy ccea"
                )
            };

            return client.SendMailAsync(
                new MailMessage(
                    from: "s12216872@stu.najah.edu",
                    to: email,
                    subject,
                    htmlMessage
                )
                {
                    IsBodyHtml = true
                }
            );
        }
    }

}
