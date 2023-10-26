using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace MailChecker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //if (ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12) == false)
            //{
            //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 ;
            //}

            string FromMail ="info@simtraxxrfid.com";
            string FromPassword = "Simtraxx2023$";
            string ToMail = "elanchezhiyan.p@aitechindia.com";
            string host = "SMTP.Office365.com";
            int port = 587;

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Create a new SmtpClient
            SmtpClient smtpClient = new SmtpClient("smtp.office365.com")
            {
                Port = 587, // Use port 587 for STARTTLS
                Credentials = new NetworkCredential(FromMail, FromPassword),
                EnableSsl = true, // Enable SSL for the STARTTLS command
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            // Create and send the email message
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(FromMail),
                Subject = "Test Email",
                Body = "This is a test email."
            };

            mailMessage.To.Add(ToMail);

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }

            //using (SmtpClient smtpClient = new SmtpClient(host, port))
            //{
            //    smtpClient.UseDefaultCredentials = false;
            //    smtpClient.Credentials = new NetworkCredential(FromMail, FromPassword);
            //    //smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword, "simtraxxrfid.com");
            //    smtpClient.EnableSsl = false;

            //    using (MailMessage mailMessage = new MailMessage())
            //    {
            //        mailMessage.From = new MailAddress(FromMail);
            //        mailMessage.To.Add(ToMail);
            //        mailMessage.Subject = "Testemail";
            //        mailMessage.Body = "This is a test email";
            //        mailMessage.IsBodyHtml = false;
            //        try
            //        {
            //            smtpClient.Send(mailMessage);
            //        }
            //        catch (Exception ex)
            //        {
            //            // Handle any exceptions that occur during email sending
            //            Console.WriteLine("Error sending email: " + ex.Message);
            //        }
            //    }
            //}

            //using (var client = new System.Net.Mail.SmtpClient(host, port))
            //{
            //    client.UseDefaultCredentials = false;
            //    client.EnableSsl = true;
            //    client.Credentials = new NetworkCredential(FromMail, FromPassword);

            //    // Connect and log in to the IMAP server
            //    client.Connect();

            //    // Perform further actions with the IMAP client as needed

            //    // Disconnect from the IMAP server
            //    client.Disconnect();
            //}


            //            var message = new MimeMessage();
            //            message.From.Add(new MailboxAddress("xxx", FromMail));
            //            message.To.Add(new MailboxAddress("xxx", ToMail));
            //            message.Subject = "How you doin'?";

            //            message.Body = new TextPart("plain")
            //            {
            //                Text = @"Hey Chandler,

            //I just wanted to let you know that Monica and I were going to go play some paintball, you in?

            //-- Joey"
            //            };

            //            using (var client = new SmtpClient(host, port))
            //            {
            //                client.EnableSsl = false;

            //                // Note: only needed if the SMTP server requires authentication
            //                client.Credentials = new NetworkCredential(FromMail , FromPassword);

            //                client.Send(message.From.ToString(), message.To.ToString(), message.Subject, message.Body.ToString());
            //            }

            using (var client = new ImapClient())
            {
                client.Connect(host, 993, true);

                client.Authenticate(FromMail, FromPassword);

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                Console.WriteLine("Total messages: {0}", inbox.Count);
                Console.WriteLine("Recent messages: {0}", inbox.Recent);

                for (int i = 0; i < inbox.Count; i++)
                {
                    var message = inbox.GetMessage(i);
                    Console.WriteLine("Subject: {0}", message.Subject);
                }

                client.Disconnect(true);


            }
        }
    }
}
