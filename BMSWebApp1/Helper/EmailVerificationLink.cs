using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace BMSWebApp1.Helper
{
    public class EmailVerificationLink
    {
        public static void EmailLinkGenerator(string email,string data, string purpose)
        {

            var fromEmail = new MailAddress("automated.bookmyshow@gmail.com", "BMS");
            var fromEmailPassword = "Psiog@123"; // Replace with actual password
            var toEmail = new MailAddress(email);


            string subject = "";
            string body = "";
            switch (purpose) {
                case "AccountVerification":
                subject = "Your account has been created.";
                body = "<br/><br/>Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + data + "'> Verify Account</a> ";
                break;

                case "ResetPassword":
                subject = "Reset your account password.";
                body = "<br/><br/>Please click on the link below to reset your password" +
                    "<br/><br/><a href=" + data + ">Reset Password link</a>";
                break;

                case "PostResetPassword":
                subject = "Your password has been reset.";
                body = "<br/><br/>You have successfully reset your account password." +
                        "<br/><br/>";
                break;

                case "BookingConfirmation":
                subject = "Booking Confirmation";
                body = "<br/><br/>" +data+
                        "<br/><br/>";
                break;

                case "AccountUpdated":
                subject = "Account details updated";
                body = "<br/><br/>You have successfully updated your account details.<br/><br/>";
                break;
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
        //public static string BookingConfirmationHTML()
        //{
        //    return @"";
        //}    
    }
}