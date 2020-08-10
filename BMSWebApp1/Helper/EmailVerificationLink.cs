using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace BMSWebApp1.Helper
{
    public class EmailVerificationLink
    {
        public static void EmailLinkGenerator(string email, string activationCode, string link, string purpose = "AccountVerification")
        {

            var fromEmail = new MailAddress("automated.bookmyshow@gmail.com", "BMS");
            var fromEmailPassword = "Psiog@123"; // Replace with actual password
            var toEmail = new MailAddress(email);


            string subject = "";
            string body = "";
            if (purpose == "AccountVerification")
            {
                subject = "Your account has been created.";
                body = "<br/><br/>Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (purpose == "ResetPassword")
            {
                subject = "Reset your account password.";
                body = "Hi,<br/><br/>Please click on the link below to reset your password" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";
            }


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }
}